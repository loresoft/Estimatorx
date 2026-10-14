namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class UserUpdateModel : EntityUpdateModel
{
    public string? Email { get; set; }

    public string? Name { get; set; }

    public bool IsAdministrator { get; set; }

    public bool IsDisabled { get; set; }
}
