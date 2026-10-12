#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.Mapping;

using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class InviteToInviteReadModelMapper
    : MapperProfile<Entities.Invite, Models.InviteReadModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Invite, Models.InviteReadModel> mapping)
    {
        // custom mapping here
        mapping
            .Property(d => d.OrganizationName)
            .From(s => s.Organization.Name);
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class InviteToInviteUpdateModelMapper
    : MapperProfile<Entities.Invite, Models.InviteUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Invite, Models.InviteUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class InviteCreateModelToInviteMapper
    : MapperProfile<Models.InviteCreateModel, Entities.Invite>
{
    protected override void ConfigureMapping(MappingBuilder<Models.InviteCreateModel, Entities.Invite> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class InviteUpdateModelToInviteMapper
    : MapperProfile<Models.InviteUpdateModel, Entities.Invite>
{
    protected override void ConfigureMapping(MappingBuilder<Models.InviteUpdateModel, Entities.Invite> mapping)
    {
        // custom mapping here
    }
}

