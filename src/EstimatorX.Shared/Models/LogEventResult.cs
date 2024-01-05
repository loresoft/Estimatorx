namespace EstimatorX.Shared.Models;

public class LogEventResult
{
    public string ContinuationToken { get; set; }

    public IReadOnlyCollection<LogEvent> Data { get; set; }
}
