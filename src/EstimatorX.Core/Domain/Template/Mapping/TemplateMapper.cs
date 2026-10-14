#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.Mapping;

using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class TemplateToTemplateReadModelMapper
    : MapperProfile<Entities.Template, Models.TemplateReadModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Template, Models.TemplateReadModel> mapping)
    {
        // custom mapping here
        mapping
            .Property(d => d.OrganizationName)
            .From(s => s.Organization!.Name);

        mapping
            .Property(d => d.UserName)
            .From(s => s.User!.Name);

        mapping
            .Property(d => d.UserEmail)
            .From(s => s.User!.Email);
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class TemplateToTemplateListModelMapper
    : MapperProfile<Entities.Template, Models.TemplateListModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Template, Models.TemplateListModel> mapping)
    {
        // custom mapping here
        mapping
            .Property(d => d.OrganizationName)
            .From(s => s.Organization!.Name);

        mapping
            .Property(d => d.UserName)
            .From(s => s.User!.Name);

        mapping
            .Property(d => d.UserEmail)
            .From(s => s.User!.Email);
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class TemplateToTemplateUpdateModelMapper
    : MapperProfile<Entities.Template, Models.TemplateUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Template, Models.TemplateUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class TemplateCreateModelToTemplateMapper
    : MapperProfile<Models.TemplateCreateModel, Entities.Template>
{
    protected override void ConfigureMapping(MappingBuilder<Models.TemplateCreateModel, Entities.Template> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class TemplateUpdateModelToTemplateMapper
    : MapperProfile<Models.TemplateUpdateModel, Entities.Template>
{
    protected override void ConfigureMapping(MappingBuilder<Models.TemplateUpdateModel, Entities.Template> mapping)
    {
        // custom mapping here
    }
}

