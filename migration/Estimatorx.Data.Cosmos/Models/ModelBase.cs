namespace Estimatorx.Data.Cosmos.Models;


public partial class ModelBase
{
    public string Id { get; set; }

    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;

    public string UpdatedBy { get; set; }
}
