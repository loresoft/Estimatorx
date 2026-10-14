using System.ComponentModel.DataAnnotations.Schema;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class UserOrganizationReadModel : EntityReadModel
{
    public long UserId { get; set; }

    [Column("User.Email")]
    public string? UserEmail { get; set; }

    [Column("User.Name")]
    public string? UserName { get; set; }

    public long OrganizationId { get; set; }

    [Column("Organization.Name")]
    public string OrganizationName { get; set; } = null!;

    public bool IsOwner { get; set; }


    public override string ToString()
        => UserName ?? UserEmail ?? UserId.ToString();
}
