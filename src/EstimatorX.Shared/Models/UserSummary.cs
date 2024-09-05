namespace EstimatorX.Shared.Models;

[Equatable]
public partial class UserSummary : ModelBase
{
    public string Name { get; set; }

    public string Email { get; set; }
}
