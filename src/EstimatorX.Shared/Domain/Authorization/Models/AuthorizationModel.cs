#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EstimatorX.Domain.Models;

[MessagePackObject(true)]
public class AuthorizationModel
{
    public long UserId { get; set; }
    public string? Name { get; set; }
    public string? Email{ get; set; }

    public bool AllowAdministrator { get; set; }
}
