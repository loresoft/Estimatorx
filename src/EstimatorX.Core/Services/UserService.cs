using System.Security.Principal;

using AutoMapper;

using Cosmos.Abstracts;

using EstimatorX.Core.Query;
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public class UserService : ServiceBase<IUserRepository, User>, IUserService, ITransientService
{
    public UserService(ILoggerFactory loggerFactory, IMapper mapper, IUserRepository repository, IUserCache userCache)
        : base(loggerFactory, mapper, repository, userCache)
    {
    }

    public override async Task Delete(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        string userId = principal.GetUserId();

        var user = await Repository.FindAsync(id, partitionKey, cancellationToken);
        if (user == null)
            return;

        if (user.Id != userId)
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized to delete this user");

        await Repository.DeleteAsync(id, cancellationToken: cancellationToken);
    }

    public override async Task<User> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var user = await Repository.FindAsync(id, partitionKey, cancellationToken);
        if (user == null)
            return null; // throw NotFound?

        return user;
    }

    public override async Task<User> Save(string id, string partitionKey, User model, IPrincipal principal, CancellationToken cancellationToken)
    {
        string userId = principal.GetUserId();

        var user = await Repository.FindAsync(model.Id, cancellationToken: cancellationToken);
        if (user != null && user.Id != userId)
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized to update this user");

        if (user == null)
        {
            user = new User
            {
                Id = userId,
                PrivateKey = ObjectId.GenerateNewId()
            };
        }

        Mapper.Map(model, user);

        UpdateTracking(user, principal);

        var result = await Repository.SaveAsync(user, cancellationToken);
        if (result == null)
            throw new DomainException(System.Net.HttpStatusCode.InternalServerError, "Failed to save user");

        return result;
    }

    public override async Task<User> Create(User model, IPrincipal principal, CancellationToken cancellationToken)
    {
        string userId = principal.GetUserId();

        var user = new User
        {
            Id = userId,
            PrivateKey = ObjectId.GenerateNewId()
        };

        Mapper.Map(model, user);

        UpdateTracking(user, principal);

        var result = await Repository.SaveAsync(user, cancellationToken);
        if (result == null)
            throw new DomainException(System.Net.HttpStatusCode.InternalServerError, "Failed to save user");

        return result;
    }

    public override async Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken)
    {
        var query = await Repository.GetQueryableAsync();

        var securyQuery = query as IQueryable<User>;

        if (queryRequest.Search.HasValue())
        {
            securyQuery = securyQuery
                .Where(u =>
                    u.Name.Contains(queryRequest.Search, StringComparison.InvariantCultureIgnoreCase) ||
                    u.Email.Contains(queryRequest.Search, StringComparison.InvariantCultureIgnoreCase)
                );
        }

        return await securyQuery
            .ToDataResult<User, TResult>(
                config => config
                    .Request(queryRequest)
                    .Mapper(Mapper.ConfigurationProvider),
                cancellationToken
            );
    }

    public async Task<UserProfile> Login(BrowserDetail browserDetail, IPrincipal principal, CancellationToken cancellationToken)
    {
        if (principal.Identity?.IsAuthenticated != true)
            return new UserProfile();

        var userId = principal.GetUserId();
        if (userId == null)
            return new UserProfile();

        var user = await Repository.FindAsync(userId, cancellationToken: cancellationToken);
        if (user == null)
        {
            user = new User
            {
                Id = userId,
                PrivateKey = ObjectId.GenerateNewId(),
                Name = principal.GetName(),
                Email = principal.GetEmail(),
                Provider = principal.GetProvider()
            };
        }

        user.Logins.Add(browserDetail);

        // only keep latest 10 logins
        while (user.Logins.Count > 10)
            user.Logins.RemoveAt(0);

        UpdateTracking(user, principal);

        var savedEntity = await Repository.SaveAsync(user, cancellationToken);
        if (savedEntity == null)
            throw new DomainException(System.Net.HttpStatusCode.InternalServerError, "Failed to save user");

        return Mapper.Map<UserProfile>(savedEntity);
    }

}
