namespace EstimatorX.Data;

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public const string DefaultConnectionString = "Data Source=/data/EstimatorX.db";


    public string? Provider { get; set; }

    public string? ConnectionString { get; set; }

    public bool MigrateOnStartup { get; set; } = true;
}
