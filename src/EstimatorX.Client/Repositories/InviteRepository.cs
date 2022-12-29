using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

using FluentRest;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EstimatorX.Client.Repositories;

[RegisterScoped]
public class InviteRepository : RepositorySearchBase<Invite, InviteSummary>
{
    public InviteRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/invite";
    }

    public async Task Send(string id, string partitionKey)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        try
        {
            var result = await Gateway.PostAsync(b => b
                .AppendPath(GetBasePath())
                .AppendPath(id)
                .AppendPath(partitionKey)
                .AppendPath("send")
            );

            result.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return;
    }

    public async Task Redeem(string securityKey)
    {
        if (securityKey is null)
            throw new ArgumentNullException(nameof(securityKey));

        try
        {
            var result = await Gateway.PostAsync(b => b
                .AppendPath(GetBasePath())
                .AppendPath("key")
                .AppendPath(securityKey)
                .AppendPath("redeem")
            );

            result.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return;
    }

    public async Task<Invite> LoadByKey(string securityKey)
    {
        if (securityKey is null)
            throw new ArgumentNullException(nameof(securityKey));

        try
        {
            return await Gateway.GetAsync<Invite>(b => b
                .AppendPath(GetBasePath())
                .AppendPath("key")
                .AppendPath(securityKey)
            );
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
            return default;
        }
    }
}
