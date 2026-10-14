#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserProviderReadModelToUserProviderCreateModelMapper
    : MapperProfile<Models.UserProviderReadModel, Models.UserProviderCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserProviderReadModel, Models.UserProviderCreateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserProviderReadModelToUserProviderUpdateModelMapper
    : MapperProfile<Models.UserProviderReadModel, Models.UserProviderUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserProviderReadModel, Models.UserProviderUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class UserProviderUpdateModelToUserProviderCreateModelMapper
    : MapperProfile<Models.UserProviderUpdateModel, Models.UserProviderCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.UserProviderUpdateModel, Models.UserProviderCreateModel> mapping)
    {
        // custom mapping here
    }
}

