namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class UserProviderUpdateModel : EntityUpdateModel
{
    public string Provider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public long UserId { get; set; }
}
