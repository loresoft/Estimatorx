namespace Estimatorx.Data.Cosmos.Models;


public partial class User : ModelBase
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Provider { get; set; }

    public string PrivateKey { get; set; }

    
    public List<string> Roles { get; set; } = new();
    
    public List<IdentifierName> Organizations { get; set; } = new();
    
    public List<BrowserDetail> Logins { get; set; } = new();


    public long? Key { get; set; }
}
