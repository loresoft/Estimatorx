
using System.Security.Principal;

using AutoMapper;

using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public class ProjectService : OrganizationServiceBase<IProjectRepository, Project>, IProjectService, ITransientService
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IProjectBuilder _projectBuilder;

    public ProjectService(ILoggerFactory loggerFactory, IMapper mapper, IProjectRepository repository, IUserCache userCache, ITemplateRepository templateRepository, IProjectBuilder projectBuilder)
        : base(loggerFactory, mapper, repository, userCache)
    {
        _templateRepository = templateRepository;
        _projectBuilder = projectBuilder;
    }

    public override async Task<Project> Create(Project model, IPrincipal principal, CancellationToken cancellationToken)
    {
        if (model.TemplateKey?.HasValue() == true)
        {
            // apply template
            CosmosKey.TryDecode(model.TemplateKey, out var id, out var key);

            var template = await _templateRepository.FindAsync(id, key);
            if (template != null)
                Mapper.Map(template, model);
        }
        else
        {
            _projectBuilder.UpdateProject(model, true);
        }

        return await base.Create(model, principal, cancellationToken);
    }

    protected override IQueryable<Project> SearchQuery(IQueryable<Project> query, string searchTerm)
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
