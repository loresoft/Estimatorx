
using System.Security.Principal;

using AutoMapper;

using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

[RegisterTransient]
public class TemplateService : OrganizationServiceBase<ITemplateRepository, Template>, ITemplateService
{
    private readonly IProjectBuilder _projectBuilder;
    private readonly IProjectCalculator _projectCalculator;

    public TemplateService(ILoggerFactory loggerFactory, IMapper mapper, ITemplateRepository repository, IUserCache userCache, IProjectBuilder projectBuilder, IProjectCalculator projectCalculator)
        : base(loggerFactory, mapper, repository, userCache)
    {
        _projectBuilder = projectBuilder;
        _projectCalculator = projectCalculator;
    }

    public override Task<Template> Save(string id, string partitionKey, Template model, IPrincipal principal, CancellationToken cancellationToken)
    {
        // ensure valid settings
        _projectBuilder.UpdateSettings(model.Settings);

        // re-calculate the computed values
        _projectCalculator.UpdateProject(model);

        return base.Save(id, partitionKey, model, principal, cancellationToken);
    }

    public override Task<Template> Create(Template model, IPrincipal principal, CancellationToken cancellationToken)
    {
        // ensure valid settings
        _projectBuilder.UpdateSettings(model.Settings, true);

        // re-calculate the computed values
        _projectCalculator.UpdateProject(model);

        return base.Create(model, principal, cancellationToken);
    }

    protected override IQueryable<Template> SearchQuery(IQueryable<Template> query, string searchTerm)
    {
        if (searchTerm.IsNullOrWhiteSpace())
            return query;

        return query
            .Where(u =>
                u.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                u.Description.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase)
            );
    }
}
