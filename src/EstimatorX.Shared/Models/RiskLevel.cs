namespace EstimatorX.Shared.Models;

[Equatable]
public partial class RiskLevel
{
    public string Risk { get; set; }

    public double Multiplier { get; set; }
}
