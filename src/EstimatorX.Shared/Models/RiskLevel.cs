namespace EstimatorX.Shared.Models;

public class RiskLevel
{
    public string Risk { get; set; }

    public double Multiplier { get; set; }


    public override int GetHashCode()
    {
        return HashCode.Combine(Risk, Multiplier);
    }
}
