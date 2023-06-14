using System.Text.Json;

using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;

using Json.Patch;

namespace EstimatorX.Client.Stores;

public abstract class StoreEditBase<TModel, TRepository> : StoreBase<TModel>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : class, IHaveIdentifier, new()
{

    protected StoreEditBase(
        ILoggerFactory loggerFactory,
        TRepository repository,
        JsonSerializerOptions serializerOptions) : base(loggerFactory)
    {
        Repository = repository;
        SerializerOptions = serializerOptions;
    }

    public JsonSerializerOptions SerializerOptions { get; }

    public TRepository Repository { get; protected set; }

    public TModel Original { get; protected set; }

    public int OriginalHash { get; protected set; }

    public bool IsBusy { get; protected set; }

    public bool IsDirty => Model?.GetHashCode() != OriginalHash;

    public bool IsClean => Model?.GetHashCode() == OriginalHash;

    public override void Set(TModel model)
    {
        OriginalHash = model.GetHashCode();
        Model = model;
        SetOriginal(model);

        Logger.LogDebug("Store model '{modelType}' changed.", typeof(TModel).Name);
        NotifyStateChanged();
    }

    public override void Clear()
    {
        OriginalHash = 0;
        Model = default;
        SetOriginal(default);

        Logger.LogDebug("Store model '{modelType}' cleared.", typeof(TModel).Name);
        NotifyStateChanged();
    }

    public override void New()
    {
        Model = new TModel();
        SetOriginal(Model);

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

            var model = await Repository.Load(id, partitionKey);

            Model = model;
            SetOriginal(model);

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
            TModel model;

            if (id.HasValue())
            {
                var jsonPatch = Original.CreatePatch(Model, SerializerOptions);
                model = await Repository.Patch(jsonPatch, id, partitionKey);
            }
            else
            {
                model = await Repository.Create(Model);
            }

            Model = model;
            SetOriginal(model);

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

            Model = default;
            SetOriginal(default);
        }
        finally
        {
            IsBusy = false;
            NotifyStateChanged();
        }
    }


    protected void SetOriginal(TModel model)
    {
        if (model == null)
        {
            OriginalHash = default;
            Original = default;
        }
        else
        {
            Original = model.JsonClone(SerializerOptions);
            OriginalHash = model.GetHashCode();
        }
    }

}
