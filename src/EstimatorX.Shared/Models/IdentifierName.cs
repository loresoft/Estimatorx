using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

[Equatable]
public partial class IdentifierName : IHaveIdentifier, IHaveName
{
    public string Id { get; set; }

    public string Name { get; set; }
}
