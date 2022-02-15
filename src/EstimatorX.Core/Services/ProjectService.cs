
using System.Security.Principal;

using AutoMapper;

using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public class ProjectService : OrganizationServiceBase<IProjectRepository, Project>, IProjectService, IServiceTransient
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IProjectBuilder _projectBuilder;
    private readonly IProjectCalculator _projectCalculator;
    private readonly ISecurityKeyGenerator _securityKeyGenerator;

    public ProjectService(
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IProjectRepository repository,
        IUserCache userCache,
        ITemplateRepository templateRepository,
        IProjectBuilder projectBuilder,
        IProjectCalculator projectCalculator,
        ISecurityKeyGenerator securityKeyGenerator)
        : base(loggerFactory, mapper, repository, userCache)
    {
        _templateRepository = templateRepository;
        _projectBuilder = projectBuilder;
        _projectCalculator = projectCalculator;
        _securityKeyGenerator = securityKeyGenerator;
    }

    public override Task<Project> Save(string id, string partitionKey, Project model, IPrincipal principal, CancellationToken cancellationToken)
    {
        if (model.SecurityKey.IsNullOrWhiteSpace())
            model.SecurityKey = _securityKeyGenerator.GenerateKey();

        // ensure valid settings
        _projectBuilder.UpdateSettings(model.Settings);

        // re-calculate the computed values
        _projectCalculator.UpdateProject(model);

        return base.Save(id, partitionKey, model, principal, cancellationToken);
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

            // ensure valid settings
            _projectBuilder.UpdateSettings(model.Settings);
        }
        else
        {
            _projectBuilder.UpdateProject(model, true);
        }

        // re-calculate the computed values
        _projectCalculator.UpdateProject(model);

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
