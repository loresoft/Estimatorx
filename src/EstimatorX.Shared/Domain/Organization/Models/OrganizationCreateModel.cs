namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class OrganizationCreateModel : EntityCreateModel
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [SequenceEquality]
    public IList<string>? HostMatches { get; set; }
}
