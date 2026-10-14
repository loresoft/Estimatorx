#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.CommandQuery.EntityFramework;

using Microsoft.Extensions.DependencyInjection;

using Context = EstimatorX.Data;
using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain;

public static class TemplateServiceRegistration
{
    [RegisterServices]
    public static void Register(IServiceCollection services)
    {
        services.AddEntityQueries<Context.EstimatorContext, Entities.Template, long, Models.TemplateReadModel>();
        services.AddEntityQueries<Context.EstimatorContext, Entities.Template, long, Models.TemplateListModel>();
        services.AddEntityCommands<Context.EstimatorContext, Entities.Template, long, Models.TemplateReadModel, Models.TemplateCreateModel, Models.TemplateUpdateModel>();
    }
}
