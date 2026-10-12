namespace Estimatorx.Data.Cosmos.Models;


public partial class FeatureEstimate : EstimateBase
{
    
    public List<StoryEstimate> Stories { get; set; } = new();
}
