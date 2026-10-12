using Arbiter.CommandQuery.Extensions;
using Arbiter.CommandQuery.Handlers;
using Arbiter.Mapping;

using EstimatorX.Data;
using EstimatorX.Data.Entities;
using EstimatorX.Domain.Models;
using EstimatorX.Domain.Queries;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Domain.Handlers;

[RegisterTransient]
public class ProjectReportHandler : RequestHandlerBase<ProjectReportQuery, ProjectReadModel>
{
    private readonly EstimatorContext _context;
    private readonly IMapper _mapper;

    public ProjectReportHandler(ILoggerFactory loggerFactory, EstimatorContext context, IMapper mapper)
        : base(loggerFactory)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override async ValueTask<ProjectReadModel?> Process(ProjectReportQuery request, CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "Querying Project with SecurityKey {SecurityKey}",
            request.SecurityKey);

        return await _context.Projects
            .AsNoTracking()
            .Where(x => x.SecurityKey == request.SecurityKey)
            .ProjectTo<Project, ProjectReadModel>(_mapper)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
