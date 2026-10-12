#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class InviteReadModelToInviteCreateModelMapper
    : MapperProfile<Models.InviteReadModel, Models.InviteCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.InviteReadModel, Models.InviteCreateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class InviteReadModelToInviteUpdateModelMapper
    : MapperProfile<Models.InviteReadModel, Models.InviteUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.InviteReadModel, Models.InviteUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class InviteUpdateModelToInviteCreateModelMapper
    : MapperProfile<Models.InviteUpdateModel, Models.InviteCreateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Models.InviteUpdateModel, Models.InviteCreateModel> mapping)
    {
        // custom mapping here
    }
}

