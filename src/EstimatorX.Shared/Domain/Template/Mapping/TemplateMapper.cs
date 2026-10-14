#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class TemplateReadModelToTemplateCreateModelMapper
    : MapperProfile<Models.TemplateReadModel, Models.TemplateCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.TemplateReadModel, Models.TemplateCreateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class TemplateReadModelToTemplateUpdateModelMapper
    : MapperProfile<Models.TemplateReadModel, Models.TemplateUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.TemplateReadModel, Models.TemplateUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class TemplateUpdateModelToTemplateCreateModelMapper
    : MapperProfile<Models.TemplateUpdateModel, Models.TemplateCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.TemplateUpdateModel, Models.TemplateCreateModel> mapping)
    {
        // custom mapping here
    }
}
