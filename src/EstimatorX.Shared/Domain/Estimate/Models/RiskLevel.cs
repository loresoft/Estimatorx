namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class RiskLevel
{
    public string Risk { get; set; } = null!;

    public double Multiplier { get; set; }

    public override string ToString() => Risk;
}
