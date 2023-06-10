namespace EstimatorX.Shared.Changes;

public static class ChangeFeedConstants
{
    public const string HubName = "ChangeFeedHub";
    public const string HubPath = "/hub/changeFeed";

    public const string LeaseContainer = "ChangeFeedLease";
    public const string LeasePartitionKey = "/id";

    public const string ProjectChangeEventName = "projectChanged";
    public const string TemplateChangeEventName = "templateChanged";
}
