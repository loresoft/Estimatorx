#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserReadModelToUserCreateModelMapper
    : MapperProfile<Models.UserReadModel, Models.UserCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserReadModel, Models.UserCreateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserReadModelToUserUpdateModelMapper
    : MapperProfile<Models.UserReadModel, Models.UserUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserReadModel, Models.UserUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserUpdateModelToUserCreateModelMapper
    : MapperProfile<Models.UserUpdateModel, Models.UserCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserUpdateModel, Models.UserCreateModel> mapping)
    {
        // custom mapping here
    }
}

