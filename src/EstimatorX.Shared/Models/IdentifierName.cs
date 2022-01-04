using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class IdentifierName : IHaveIdentifier, IHaveName
{
    public string Id { get; set; }

    public string Name { get; set; }
   

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }
}
