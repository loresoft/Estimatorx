namespace EstimatorX.Shared.Models;

public class ProjectOverhead
{
    public string Name { get; set; }

    public string Description { get; set; }

    public double Multiplier { get; set; }


    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Description, Multiplier);
    }
}
