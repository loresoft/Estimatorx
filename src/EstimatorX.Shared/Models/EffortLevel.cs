namespace EstimatorX.Shared.Models;

public class EffortLevel
{
    public int Effort { get; set; }

    public string Level { get; set; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Effort, Level);
    }
}
