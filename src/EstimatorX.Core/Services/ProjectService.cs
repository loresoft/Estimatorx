
using AutoMapper;

using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public class ProjectService : OrganizationServiceBase<IProjectRepository, Project>, IProjectService, ITransientService
{
    public ProjectService(ILoggerFactory loggerFactory, IMapper mapper, IProjectRepository repository, IUserCache userCache)
        : base(loggerFactory, mapper, repository, userCache)
    {
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
