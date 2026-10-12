using System.ComponentModel.DataAnnotations.Schema;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class UserLoginReadModel : EntityReadModel
{
    public long UserId { get; set; }

    [Column("User.Name")]
    public string? UserName { get; set; }

    [Column("User.Email")]
    public string? UserEmail { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }
}
