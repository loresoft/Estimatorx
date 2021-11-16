namespace EstimatorX.Core.Comparison;

public class Delta<T>
{
    public IReadOnlyCollection<T> Matched { get; set; }
    public IReadOnlyCollection<T> Created { get; set; }
    public IReadOnlyCollection<T> Deleted { get; set; }
}
