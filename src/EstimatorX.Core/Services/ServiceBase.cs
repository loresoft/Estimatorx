using System.Security.Principal;

using AutoMapper;

using EstimatorX.Core.Entities;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public abstract class ServiceBase<TRepository, TModel> : IService<TModel>
{

    protected ServiceBase(ILoggerFactory loggerFactory, IMapper mapper, TRepository repository, IUserCache userCache)
    {
        Logger = loggerFactory.CreateLogger(GetType());
        Mapper = mapper;
        Repository = repository;
        UserCache = userCache;
    }

    protected ILogger Logger { get; }

    protected IMapper Mapper { get; }

    protected TRepository Repository { get; }

    protected IUserCache UserCache { get; }


    public abstract Task Delete(string id, IPrincipal principal, CancellationToken cancellationToken);

    public abstract Task<TModel> Load(string id, IPrincipal principal, CancellationToken cancellationToken);

    public abstract Task<TModel> Save(TModel model, IPrincipal principal, CancellationToken cancellationToken);

    public abstract Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryModel, IPrincipal principal, CancellationToken cancellationToken);


    protected T UpdateTracking<T>(T model, IPrincipal principal)
        where T : EntityBase
    {
        string userName = principal.GetEmail();

        if (model.Created == default)
            model.Created = DateTimeOffset.UtcNow;

        if (model.CreatedBy.IsNullOrEmpty())
            model.CreatedBy = userName;

        model.Updated = DateTimeOffset.UtcNow;
        model.UpdatedBy = userName;

        return model;
    }

    protected async Task<User> CurrentUser(IPrincipal principal, CancellationToken cancellationToken = default)
    {
        string userId = principal.GetUserId();

        return userId.HasValue()
            ? await UserCache.Get(userId, cancellationToken)
            : default;
    }
}
