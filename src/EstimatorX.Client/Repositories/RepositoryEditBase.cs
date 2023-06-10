using EstimatorX.Client.Services;
using EstimatorX.Shared.Extensions;

using FluentRest;

using Json.Patch;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EstimatorX.Client.Repositories;

public abstract class RepositoryEditBase<TModel>
{
    protected RepositoryEditBase(GatewayClient gateway)
    {
        Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
    }

    protected GatewayClient Gateway { get; }

    public async Task Delete(string id, string partitionKey = null)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        try
        {
            var result = await Gateway.DeleteAsync(b => b
                .AppendPath(GetBasePath())
                .AppendPath(id)
                .AppendPathIf(partitionKey.HasValue, partitionKey)
            );

            result.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return;
    }


    public async Task<TModel> Load(string id, string partitionKey = null)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        try
        {
            return await Gateway.GetAsync<TModel>(b => b
                .AppendPath(GetBasePath())
                .AppendPath(id)
                .AppendPathIf(partitionKey.HasValue, partitionKey)
            );
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
            return default;
        }
    }

    public async Task<TModel> Save(TModel model, string id, string partitionKey = null)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        try
        {
            return await Gateway.PostAsync<TModel>(b => b
                .AppendPath(GetBasePath())
                .AppendPath(id)
                .AppendPathIf(partitionKey.HasValue, partitionKey)
                .Content(model)
            );
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
            return default;
        }
    }

    public async Task<TModel> Create(TModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        try
        {
            return await Gateway.PostAsync<TModel>(b => b
                .AppendPath(GetBasePath())
                .Content(model)
            );
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
            return default;
        }
    }

    public async Task<TModel> Patch(JsonPatch patch, string id, string partitionKey = null)
    {
        if (patch == null)
            throw new ArgumentNullException(nameof(patch));

        try
        {
            return await Gateway.PatchAsync<TModel>(b => b
                .AppendPath(GetBasePath())
                .AppendPath(id)
                .AppendPathIf(partitionKey.HasValue, partitionKey)
                .Content(patch)
            );
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
            return default;
        }
    }

    protected abstract string GetBasePath();
}
