namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class UserOrganizationCreateModel : EntityCreateModel
{
    public long UserId { get; set; }

    public long OrganizationId { get; set; }

    public bool IsOwner { get; set; }
}
