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
public class UserOrganizationQueryHandler : RequestHandlerBase<UserOrganizationQuery, UserOrganizationReadModel>
{
    private readonly EstimatorContext _context;
    private readonly IMapper _mapper;

    public UserOrganizationQueryHandler(ILoggerFactory loggerFactory, EstimatorContext context, IMapper mapper) : base(loggerFactory)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override async ValueTask<UserOrganizationReadModel?> Process(UserOrganizationQuery request, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation(
            "Querying UserOrganization with UserId {UserId} and OrganizationId {OrganizationId}",
            request.UserId, request.OrganizationId);

        return await _context.UserOrganizations
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId && x.OrganizationId == request.OrganizationId)
            .ProjectTo<UserOrganization, UserOrganizationReadModel>(_mapper)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
