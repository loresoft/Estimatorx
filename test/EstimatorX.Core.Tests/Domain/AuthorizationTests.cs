using System.Security.Claims;

using Arbiter.CommandQuery.Services;
using Arbiter.Mediation;

using EstimatorX.Data;
using EstimatorX.Domain.Commands;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core.Tests.Domain;

[Collection(DatabaseCollection.CollectionName)]
public class AuthorizationTests(DatabaseFixture fixture) : TestHostBase<DatabaseFixture>(fixture)
{
    [Fact]
    public async Task NewUser_CreatesUserProviderAndLogin()
    {
        var email = UniqueEmail();
        var key = Guid.NewGuid().ToString("N");

        var model = await Send(email, "New User", "Google", key);
        model.Should().NotBeNull();
        model.Email.Should().Be(email);

        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var provider = await context.UserProviders
            .SingleAsync(p => p.Provider == "Google" && p.ProviderKey == key, TestCancellation);
        provider.UserId.Should().Be(model.UserId);

        var logins = await context.UserLogins.CountAsync(l => l.UserId == model.UserId, TestCancellation);
        logins.Should().Be(1);
    }

    [Fact]
    public async Task ReturningUser_MatchedByProviderKey_PreservesEmail()
    {
        var email = UniqueEmail();
        var key = Guid.NewGuid().ToString("N");

        var first = await Send(email, "Returning", "GitHub", key);

        var changedEmail = UniqueEmail();
        var second = await Send(changedEmail, "Returning", "GitHub", key);

        second.UserId.Should().Be(first.UserId);
        second.Email.Should().Be(email);
    }

    [Fact]
    public async Task ReturningUser_MatchedByProviderKey_PreservesName()
    {
        var email = UniqueEmail();
        var key = Guid.NewGuid().ToString("N");

        var first = await Send(email, "Returning", "GitHub", key);

        var second = await Send(email, "Returning Renamed", "GitHub", key);

        second.UserId.Should().Be(first.UserId);
        second.Name.Should().Be("Returning");
    }

    [Fact]
    public async Task ExistingUser_DifferentProvider_LinkedByEmail()
    {
        var email = UniqueEmail();

        var first = await Send(email, "Linked", "Google", Guid.NewGuid().ToString("N"));
        var second = await Send(email, "Linked", "Facebook", Guid.NewGuid().ToString("N"));

        second.UserId.Should().Be(first.UserId);

        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var providers = await context.UserProviders.CountAsync(p => p.UserId == first.UserId, TestCancellation);
        providers.Should().Be(2);

        var logins = await context.UserLogins.CountAsync(l => l.UserId == first.UserId, TestCancellation);
        logins.Should().Be(2);
    }

    [Fact]
    public async Task DisabledUser_IsRejected()
    {
        var email = UniqueEmail();
        var key = Guid.NewGuid().ToString("N");

        var model = await Send(email, "Disabled", "Google", key);

        await using (var scope = Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<EstimatorContext>();
            var user = await context.Users.SingleAsync(u => u.Id == model.UserId, TestCancellation);
            user.IsDisabled = true;
            await context.SaveChangesAsync(TestCancellation);
        }

        var action = () => Send(email, "Disabled", "Google", key);
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task NewUser_WithoutEmail_CreatedByProvider()
    {
        var key = Guid.NewGuid().ToString("N");

        var first = await SendWithoutEmail("No Email", "GitHub", key);
        first.Email.Should().BeNull();

        var second = await SendWithoutEmail("No Email", "GitHub", key);
        second.UserId.Should().Be(first.UserId);

        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var logins = await context.UserLogins.CountAsync(l => l.UserId == first.UserId, TestCancellation);
        logins.Should().Be(2);
    }

    [Fact]
    public async Task WithoutEmailOrProvider_IsRejected()
    {
        var action = () => SendWithoutEmail("Anonymous", null, null);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ReturningUser_EmailChanged_KeepsLoginHistory()
    {
        var key = Guid.NewGuid().ToString("N");

        var first = await Send(UniqueEmail(), "History", "Google", key);
        var second = await Send(UniqueEmail(), "History", "Google", key);

        second.UserId.Should().Be(first.UserId);

        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var logins = await context.UserLogins.CountAsync(l => l.UserId == first.UserId, TestCancellation);
        logins.Should().Be(2);
    }

    private async Task<EstimatorX.Domain.Models.AuthorizationModel> SendWithoutEmail(string name, string? provider, string? providerKey)
    {
        await using var scope = Services.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var identity = new ClaimsIdentity("Test.Application", ClaimTypes.Name, ClaimTypes.Role);
        identity.AddClaim(new Claim(ClaimTypes.Name, name));
        identity.AddClaim(new Claim(ClaimNames.DisplayName, name));

        var command = new AuthorizationCommand(new ClaimsPrincipal(identity))
        {
            Provider = provider,
            ProviderKey = providerKey,
        };

        var result = await mediator.Send(command, TestCancellation);
        result.Should().NotBeNull();
        return result!;
    }

    private async Task<EstimatorX.Domain.Models.AuthorizationModel> Send(string email, string name, string provider, string providerKey)
    {
        await using var scope = Services.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var principal = MockClaimsPrincipal.CreatePrincipal(email, name);
        var command = new AuthorizationCommand(principal)
        {
            Provider = provider,
            ProviderKey = providerKey,
        };

        var result = await mediator.Send(command, TestCancellation);
        result.Should().NotBeNull();
        return result!;
    }

    private static string UniqueEmail() => $"auth-{Guid.NewGuid():N}@mailinator.com";
}
