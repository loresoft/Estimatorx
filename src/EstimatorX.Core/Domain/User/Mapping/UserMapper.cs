#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.Mapping;

using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserToUserReadModelMapper
    : MapperProfile<Entities.User, Models.UserReadModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.User, Models.UserReadModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserToUserUpdateModelMapper
    : MapperProfile<Entities.User, Models.UserUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.User, Models.UserUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserCreateModelToUserMapper
    : MapperProfile<Models.UserCreateModel, Entities.User>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserCreateModel, Entities.User> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserUpdateModelToUserMapper
    : MapperProfile<Models.UserUpdateModel, Entities.User>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserUpdateModel, Entities.User> mapping)
    {
        // custom mapping here
    }
}

