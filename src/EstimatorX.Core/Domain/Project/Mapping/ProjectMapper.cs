#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.Mapping;

using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain.Mapping;

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class ProjectToProjectReadModelMapper
    : MapperProfile<Entities.Project, Models.ProjectReadModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Project, Models.ProjectReadModel> mapping)
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
internal sealed partial class ProjectToProjectListModelMapper
    : MapperProfile<Entities.Project, Models.ProjectListModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Project, Models.ProjectListModel> mapping)
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
internal sealed partial class ProjectToProjectUpdateModelMapper
    : MapperProfile<Entities.Project, Models.ProjectUpdateModel>
{
    protected override void ConfigureMapping(MappingBuilder<Entities.Project, Models.ProjectUpdateModel> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class ProjectCreateModelToProjectMapper
    : MapperProfile<Models.ProjectCreateModel, Entities.Project>
{
    protected override void ConfigureMapping(MappingBuilder<Models.ProjectCreateModel, Entities.Project> mapping)
    {
        // custom mapping here
    }
}

[GenerateMapper]
[RegisterSingleton]
internal sealed partial class ProjectUpdateModelToProjectMapper
    : MapperProfile<Models.ProjectUpdateModel, Entities.Project>
{
    protected override void ConfigureMapping(MappingBuilder<Models.ProjectUpdateModel, Entities.Project> mapping)
    {
        // custom mapping here
    }
}

