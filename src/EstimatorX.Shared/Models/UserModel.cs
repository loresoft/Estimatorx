using System.Collections.Generic;

namespace EstimatorX.Shared.Models;

public class UserModel : ModelBase
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Provider { get; set; }

    public string PrivateKey { get; set; }

    public List<string> Roles { get; set; } = new();

    public List<string> Organizations { get; set; } = new();
}
