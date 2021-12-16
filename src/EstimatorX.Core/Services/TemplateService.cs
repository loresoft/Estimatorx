
using AutoMapper;

using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public class TemplateService : OrganizationServiceBase<ITemplateRepository, Template>, ITemplateService, ITransientService
{
    public TemplateService(ILoggerFactory loggerFactory, IMapper mapper, ITemplateRepository repository, IUserCache userCache)
        : base(loggerFactory, mapper, repository, userCache)
    {
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
