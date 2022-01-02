
using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;

namespace EstimatorX.Client.Stores;

public class StoreEditBase<TModel, TRepository> : StoreBase<TModel>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : class, IHaveIdentifier, new()
{

    public StoreEditBase(ILoggerFactory loggerFactory, TRepository repository) : base(loggerFactory)
    {
        Repository = repository;
    }

    public TRepository Repository { get; protected set; }

    public int OriginalHash { get; private set; }

    public bool IsBusy { get; private set; }

    public bool IsDirty => Model?.GetHashCode() != OriginalHash;

    public bool IsClean => Model?.GetHashCode() == OriginalHash;

    public override void Set(TModel model)
    {
        OriginalHash = model.GetHashCode();
        Model = model;

        Logger.LogDebug("Store model '{modelType}' changed.", typeof(TModel).Name);
        NotifyStateChanged();
    }

    public override void Clear()
    {
        OriginalHash = 0;
        Model = default;

        Logger.LogDebug("Store model '{modelType}' cleared.", typeof(TModel).Name);
        NotifyStateChanged();
    }

    public override void New()
    {
        Model = new TModel();
        OriginalHash = Model.GetHashCode();

        Logger.LogDebug("Store model '{modelType}' created.", typeof(TModel).Name);
        NotifyStateChanged();
    }

    public async Task<TModel> Load(string id, string partitionKey = null, bool force = false)
    {
        if (!force && Model?.Id == id)
            return Model;

        try
        {
            IsBusy = true;
            Model = default;

            var project = await Repository.Load(id, partitionKey);

            OriginalHash = project.GetHashCode();
            Model = project;

            return Model;
        }
        finally
        {
            IsBusy = false;
            NotifyStateChanged();
        }
    }

    public async Task<TModel> Save(string id = null, string partitionKey = null)
    {
        try
        {
            IsBusy = true;
            var project = id.HasValue()
                ? await Repository.Save(Model, id, partitionKey)
                : await Repository.Create(Model);

            OriginalHash = project.GetHashCode();
            Model = project;

            return Model;
        }
        finally
        {
            IsBusy = false;
            NotifyStateChanged();
        }
    }

    public async Task Delete(string id, string partitionKey = null)
    {
        if (Model == null)
            return;

        try
        {
            IsBusy = true;

            await Repository.Delete(id, partitionKey);

            OriginalHash = default;
            Model = default;
        }
        finally
        {
            IsBusy = false;
            NotifyStateChanged();
        }
    }
}
