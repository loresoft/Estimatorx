namespace EstimatorX.Core.Options
{
    public class HostingConfiguration
    {
        public const string ConfigurationName = "Hosting";

        public string ClientDomain { get; set; }
        public string ServiceDomain { get; set; }
        public string ServiceEndpoint { get; set; }
    }
}
