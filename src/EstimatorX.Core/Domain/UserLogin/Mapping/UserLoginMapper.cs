#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.Mapping;

using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserLoginToUserLoginReadModelMapper
    : MapperProfile<Entities.UserLogin, Models.UserLoginReadModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.UserLogin, Models.UserLoginReadModel> mapping)
    {
        // custom mapping here
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
internal sealed partial class UserLoginToUserLoginUpdateModelMapper
    : MapperProfile<Entities.UserLogin, Models.UserLoginUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.UserLogin, Models.UserLoginUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserLoginCreateModelToUserLoginMapper
    : MapperProfile<Models.UserLoginCreateModel, Entities.UserLogin>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserLoginCreateModel, Entities.UserLogin> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserLoginUpdateModelToUserLoginMapper
    : MapperProfile<Models.UserLoginUpdateModel, Entities.UserLogin>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserLoginUpdateModel, Entities.UserLogin> mapping)
    {
        // custom mapping here
    }
}

