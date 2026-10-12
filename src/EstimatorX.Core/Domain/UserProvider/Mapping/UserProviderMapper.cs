#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.Mapping;

using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserProviderToUserProviderReadModelMapper
    : MapperProfile<Entities.UserProvider, Models.UserProviderReadModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.UserProvider, Models.UserProviderReadModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserProviderToUserProviderUpdateModelMapper
    : MapperProfile<Entities.UserProvider, Models.UserProviderUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.UserProvider, Models.UserProviderUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserProviderCreateModelToUserProviderMapper
    : MapperProfile<Models.UserProviderCreateModel, Entities.UserProvider>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserProviderCreateModel, Entities.UserProvider> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserProviderUpdateModelToUserProviderMapper
    : MapperProfile<Models.UserProviderUpdateModel, Entities.UserProvider>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserProviderUpdateModel, Entities.UserProvider> mapping)
    {
        // custom mapping here
    }
}

