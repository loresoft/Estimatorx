namespace EstimatorX.Shared.Models;

public class LogEventRequest
{
    public string ContinuationToken { get; set; }

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public string Level { get; set; }

    public int PageSize { get; set; } = 100;
}
