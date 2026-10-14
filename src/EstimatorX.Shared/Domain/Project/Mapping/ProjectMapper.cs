#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Diagnostics.CodeAnalysis;

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class ProjectReadModelToProjectCreateModelMapper
    : MapperProfile<Models.ProjectReadModel, Models.ProjectCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.ProjectReadModel, Models.ProjectCreateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class ProjectReadModelToProjectUpdateModelMapper
    : MapperProfile<Models.ProjectReadModel, Models.ProjectUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.ProjectReadModel, Models.ProjectUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class ProjectUpdateModelToProjectCreateModelMapper
    : MapperProfile<Models.ProjectUpdateModel, Models.ProjectCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.ProjectUpdateModel, Models.ProjectCreateModel> mapping)
    {
        // custom mapping here
        mapping.Property(p => p.Id).Ignore();
        mapping.Property(p => p.Settings).From(source => EstimateMapper.CloneSettings(source.Settings));
        mapping.Property(p => p.Epics).From(source => source.Epics.ConvertAll(EstimateMapper.CloneEpic));
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class ProjectUpdateModelToTemplateCreateModelMapper
    : MapperProfile<Models.ProjectUpdateModel, Models.TemplateCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.ProjectUpdateModel, Models.TemplateCreateModel> mapping)
    {
        // custom mapping here
        mapping.Property(p => p.Id).Ignore();
        mapping.Property(p => p.Settings).From(source => EstimateMapper.CloneSettings(source.Settings));
        mapping.Property(p => p.Epics).From(source => source.Epics.ConvertAll(EstimateMapper.CloneEpic));
    }
}
