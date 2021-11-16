
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Entities;

public class User : EntityBase
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Provider { get; set; }

    public string PrivateKey { get; set; }

    public HashSet<string> Roles { get; set; } = new();

    public List<IdentifierName> Organizations { get; set; } = new();
}
