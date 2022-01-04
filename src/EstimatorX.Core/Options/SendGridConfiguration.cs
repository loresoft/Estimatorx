namespace EstimatorX.Core.Options;

public class SendGridConfiguration
{
    public const string ConfigurationName = "SendGrid";

    public string FromName { get; set; }
    public string FromEmail { get; set; }
    public string ApiKey { get; set; }
}
