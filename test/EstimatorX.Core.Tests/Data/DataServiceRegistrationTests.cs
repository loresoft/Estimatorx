using EstimatorX.Data;

using Microsoft.Extensions.Configuration;

namespace EstimatorX.Core.Tests.Data;

public class DataServiceRegistrationTests
{
    private static IConfiguration CreateConfiguration(params (string Key, string? Value)[] values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values.Select(v => new KeyValuePair<string, string?>(v.Key, v.Value)))
            .Build();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void ParseProvider_Empty_DefaultsToSqlite(string? value)
    {
        DataServiceRegistration.ParseProvider(value).Should().Be(DatabaseProvider.SQLite);
    }

    [Theory]
    [InlineData("SqlServer", DatabaseProvider.SqlServer)]
    [InlineData("mssql", DatabaseProvider.SqlServer)]
    [InlineData("SQLite", DatabaseProvider.SQLite)]
    [InlineData("PostgreSQL", DatabaseProvider.PostgreSQL)]
    [InlineData("postgres", DatabaseProvider.PostgreSQL)]
    [InlineData(" NPGSQL ", DatabaseProvider.PostgreSQL)]
    public void ParseProvider_KnownValues_ReturnsProvider(string value, DatabaseProvider expected)
    {
        DataServiceRegistration.ParseProvider(value).Should().Be(expected);
    }

    [Fact]
    public void ParseProvider_Unknown_Throws()
    {
        var action = () => DataServiceRegistration.ParseProvider("oracle");

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ResolveProvider_ReadsDatabaseSection()
    {
        var configuration = CreateConfiguration(("Database:Provider", "PostgreSQL"));

        DataServiceRegistration.ResolveProvider(configuration).Should().Be(DatabaseProvider.PostgreSQL);
    }

    [Fact]
    public void ResolveConnectionString_DatabaseSection_TakesPrecedence()
    {
        var configuration = CreateConfiguration(
            ("Database:ConnectionString", "Server=primary"),
            ("ConnectionStrings:EstimatorX", "Server=fallback"));

        DataServiceRegistration.ResolveConnectionString(configuration, DatabaseProvider.SqlServer)
            .Should().Be("Server=primary");
    }

    [Fact]
    public void ResolveConnectionString_FallsBackToConnectionStrings()
    {
        var configuration = CreateConfiguration(("ConnectionStrings:EstimatorX", "Server=fallback"));

        DataServiceRegistration.ResolveConnectionString(configuration, DatabaseProvider.SqlServer)
            .Should().Be("Server=fallback");
    }

    [Fact]
    public void ResolveConnectionString_Sqlite_UsesDataVolumeDefault()
    {
        var configuration = CreateConfiguration();

        DataServiceRegistration.ResolveConnectionString(configuration, DatabaseProvider.SQLite)
            .Should().Be(DatabaseOptions.DefaultConnectionString);
    }

    [Theory]
    [InlineData(DatabaseProvider.SqlServer)]
    [InlineData(DatabaseProvider.PostgreSQL)]
    public void ResolveConnectionString_MissingForServerProvider_Throws(DatabaseProvider provider)
    {
        var configuration = CreateConfiguration();

        var action = () => DataServiceRegistration.ResolveConnectionString(configuration, provider);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*Database__ConnectionString*");
    }
}
