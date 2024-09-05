using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

[Equatable]
public partial class Project : ModelBase, IHaveOrganization, IHaveName
{
    public string Name { get; set; }

    public string Description { get; set; }


    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }


    public string SecurityKey { get; set; }

    public string TemplateKey { get; set; }


    public ProjectSettings Settings { get; set; } = new();

    [SequenceEquality]
    public List<EpicEstimate> Epics { get; set; } = new();


    // computed
    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }


    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }
}
