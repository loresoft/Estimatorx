namespace EstimatorX.Shared.Definitions;

public interface ITrackCreated
{
    DateTimeOffset Created { get; set; }
    string CreatedBy { get; set; }
}
