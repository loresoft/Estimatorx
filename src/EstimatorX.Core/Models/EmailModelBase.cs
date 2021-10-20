namespace EstimatorX.Core.Models
{
    public abstract class EmailModelBase : UserAgentDetail
    {
        public string ApplicationName { get; set; } = "EstimatorX";

        public string RecipientName { get; set; }
        public string RecipientAddress { get; set; }

        public string ReplyToName { get; set; }
        public string ReplyToAddress { get; set; }

        public string Link { get; set; }
    }

}
