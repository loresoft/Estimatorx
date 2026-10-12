#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserOrganizationReadModelToUserOrganizationCreateModelMapper
    : MapperProfile<Models.UserOrganizationReadModel, Models.UserOrganizationCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserOrganizationReadModel, Models.UserOrganizationCreateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserOrganizationReadModelToUserOrganizationUpdateModelMapper
    : MapperProfile<Models.UserOrganizationReadModel, Models.UserOrganizationUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserOrganizationReadModel, Models.UserOrganizationUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserOrganizationUpdateModelToUserOrganizationCreateModelMapper
    : MapperProfile<Models.UserOrganizationUpdateModel, Models.UserOrganizationCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserOrganizationUpdateModel, Models.UserOrganizationCreateModel> mapping)
    {
        // custom mapping here
    }
}

