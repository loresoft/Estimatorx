using System.Security.Claims;

using Arbiter.CommandQuery.Endpoints;
using Arbiter.CommandQuery.Extensions;
using Arbiter.CommandQuery.Services;
using Arbiter.Mediation;

using EstimatorX.Domain.Commands;
using EstimatorX.Security;
using EstimatorX.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace EstimatorX.Web.Security;

public static class ExternalLoginEvents
{
    public static async Task OnCreatingTicket(OAuthCreatingTicketContext context)
    {
        var identity = context.Identity
            ?? throw new InvalidOperationException("External login identity is missing");

        var principal = new ClaimsPrincipal(identity);
        var command = new AuthorizationCommand(principal)
        {
            Provider = context.Scheme.Name,
            ProviderKey = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        };
        command.ApplyContext(context.HttpContext);

        var mediator = context.HttpContext.RequestServices.GetRequiredService<IMediator>();
        var model = await mediator.Send(command, context.HttpContext.RequestAborted)
            ?? throw new InvalidOperationException("Unable to resolve user for external login");

        identity.AddClaim(ClaimNames.UserId, model.UserId);
        identity.AddClaim(ClaimNames.DisplayName, model.Name);

        // allow user to override email address if they want to use a different one than the external provider
        if (model.Email.HasValue())
            identity.ReplaceClaim(ClaimTypes.Email, model.Email);

        identity.AddRole(RoleNames.Administrators, model.AllowAdministrator);
    }

    public static Task OnRemoteFailure(RemoteFailureContext context)
    {
        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(typeof(ExternalLoginEvents));

        logger.LogWarning(context.Failure, "External login failed for scheme '{Scheme}'", context.Scheme.Name);

        var redirect = RouteLinks.Account.Login(AlertService.LoginFailed);

        context.Response.Redirect(redirect);
        context.HandleResponse();

        return Task.CompletedTask;
    }
}
