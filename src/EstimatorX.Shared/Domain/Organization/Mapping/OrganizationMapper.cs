#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class OrganizationReadModelToOrganizationCreateModelMapper
    : MapperProfile<Models.OrganizationReadModel, Models.OrganizationCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.OrganizationReadModel, Models.OrganizationCreateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class OrganizationReadModelToOrganizationUpdateModelMapper
    : MapperProfile<Models.OrganizationReadModel, Models.OrganizationUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.OrganizationReadModel, Models.OrganizationUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class OrganizationUpdateModelToOrganizationCreateModelMapper
    : MapperProfile<Models.OrganizationUpdateModel, Models.OrganizationCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.OrganizationUpdateModel, Models.OrganizationCreateModel> mapping)
    {
        // custom mapping here
    }
}

