namespace EstimatorX.Shared.Definitions;

public interface IHaveIdentifier<TKey>
{
    TKey Id { get; set; }
}
