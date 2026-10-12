namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class EffortLevel
{
    public int Effort { get; set; }

    public string Level { get; set; } = null!;


    public override string ToString() => Level;
}
