namespace EstimatorX.Shared.Models;

[Equatable]
public partial class User : ModelBase
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Provider { get; set; }

    public string PrivateKey { get; set; }

    [SequenceEquality]
    public List<string> Roles { get; set; } = new();

    [SequenceEquality]
    public List<IdentifierName> Organizations { get; set; } = new();

    [SequenceEquality]
    public List<BrowserDetail> Logins { get; set; } = new();
}
