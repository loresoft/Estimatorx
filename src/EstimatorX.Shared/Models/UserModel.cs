namespace EstimatorX.Shared.Models;

public class UserModel : ModelBase
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Provider { get; set; }

    public string PrivateKey { get; set; }

    public List<string> Roles { get; set; } = new();

    public List<IdentifierName> Organizations { get; set; } = new();

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(Name);
        hash.Add(Email);
        hash.Add(Provider);
        hash.Add(PrivateKey);

        foreach (var role in Roles)
            hash.Add(role);

        foreach (var organization in Organizations)
            hash.Add(organization.GetHashCode());

        return hash.ToHashCode();
    }

}
