#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.Mapping;

using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class OrganizationToOrganizationReadModelMapper
    : MapperProfile<Entities.Organization, Models.OrganizationReadModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Organization, Models.OrganizationReadModel> mapping)
    {
        // custom mapping here
        mapping
            .Property(d => d.Owners)
            .From(s => s.UserOrganizations.Select(o => o.UserId).ToList());
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class OrganizationToOrganizationUpdateModelMapper
    : MapperProfile<Entities.Organization, Models.OrganizationUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Organization, Models.OrganizationUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class OrganizationCreateModelToOrganizationMapper
    : MapperProfile<Models.OrganizationCreateModel, Entities.Organization>
{
    protected override void ConfigureMapping(MappingBuilder<Models.OrganizationCreateModel, Entities.Organization> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class OrganizationUpdateModelToOrganizationMapper
    : MapperProfile<Models.OrganizationUpdateModel, Entities.Organization>
{
    protected override void ConfigureMapping(MappingBuilder<Models.OrganizationUpdateModel, Entities.Organization> mapping)
    {
        // custom mapping here
    }
}

