namespace EstimatorX.Shared.Models;

public class QueryResult<T>
{
    public IEnumerable<T> Data { get; set; }

    public int Total { get; set; }
}

