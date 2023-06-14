namespace EstimatorX.Shared.Models;

public class UserProfile : ModelBase
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Provider { get; set; }

    public string PrivateKey { get; set; }

    public List<string> Roles { get; set; } = new();

    public List<IdentifierName> Organizations { get; set; } = new();
}
