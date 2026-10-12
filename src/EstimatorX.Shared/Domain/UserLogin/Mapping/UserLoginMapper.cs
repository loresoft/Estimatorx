#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserLoginReadModelToUserLoginCreateModelMapper
    : MapperProfile<Models.UserLoginReadModel, Models.UserLoginCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserLoginReadModel, Models.UserLoginCreateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserLoginReadModelToUserLoginUpdateModelMapper
    : MapperProfile<Models.UserLoginReadModel, Models.UserLoginUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserLoginReadModel, Models.UserLoginUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserLoginUpdateModelToUserLoginCreateModelMapper
    : MapperProfile<Models.UserLoginUpdateModel, Models.UserLoginCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserLoginUpdateModel, Models.UserLoginCreateModel> mapping)
    {
        // custom mapping here
    }
}

