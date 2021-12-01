namespace EstimatorX.Shared.Models;

public class QueryRequest
{
    public int? Page { get; set; } = 1;

    public int? PageSize { get; set; } = 20;

    public string Sort { get; set; }

    public bool? Descending { get; set; }

    public string Search { get; set; }

    public string Organization { get; set; }
}
