namespace Estimatorx.Data.Cosmos.Models;


public partial class EpicEstimate
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string Name { get; set; }

    public string Description { get; set; }

    public string Assumptions { get; set; }

    
    public List<FeatureEstimate> Features { get; set; } = new();

    // computed
    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }

    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }
}
