#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.Mapping;

using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserOrganizationToUserOrganizationReadModelMapper
    : MapperProfile<Entities.UserOrganization, Models.UserOrganizationReadModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.UserOrganization, Models.UserOrganizationReadModel> mapping)
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
internal sealed partial class UserOrganizationToUserOrganizationUpdateModelMapper
    : MapperProfile<Entities.UserOrganization, Models.UserOrganizationUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.UserOrganization, Models.UserOrganizationUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserOrganizationCreateModelToUserOrganizationMapper
    : MapperProfile<Models.UserOrganizationCreateModel, Entities.UserOrganization>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserOrganizationCreateModel, Entities.UserOrganization> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserOrganizationUpdateModelToUserOrganizationMapper
    : MapperProfile<Models.UserOrganizationUpdateModel, Entities.UserOrganization>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserOrganizationUpdateModel, Entities.UserOrganization> mapping)
    {
        // custom mapping here
    }
}

