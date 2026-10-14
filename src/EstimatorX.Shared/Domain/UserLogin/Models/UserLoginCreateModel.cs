namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class UserLoginCreateModel : EntityCreateModel
{
    public long UserId { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }
}
