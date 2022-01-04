namespace EstimatorX.Shared.Models;

public class FeatureEstimate : EstimateBase
{
    public List<StoryEstimate> Stories { get; init; } = new();

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(base.GetHashCode());

        foreach (var story in Stories)
            hash.Add(story.GetHashCode());

        return hash.ToHashCode();
    }
}
