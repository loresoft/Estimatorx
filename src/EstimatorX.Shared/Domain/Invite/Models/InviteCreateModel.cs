namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class InviteCreateModel
    : EntityCreateModel
{
    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public long OrganizationId { get; set; }

    public string? SecurityCode { get; set; }

    public DateTimeOffset Expiration { get; set; }

    public DateTimeOffset? LastSent { get; set; }
}
