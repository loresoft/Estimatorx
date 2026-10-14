using Arbiter.CommandQuery.Definitions;
using Arbiter.CommandQuery.EntityFramework.Handlers;
using Arbiter.CommandQuery.Extensions;
using Arbiter.CommandQuery.Models;
using Arbiter.CommandQuery.Options;
using Arbiter.Communication.Email;
using Arbiter.Mapping;
using Arbiter.Services;

using EstimatorX.Data;
using EstimatorX.Data.Queries;
using EstimatorX.Domain.Commands;
using EstimatorX.Domain.Models;
using EstimatorX.Templates;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Domain.Handlers;

[RegisterTransient]
public class InviteSendHandler : DataContextHandlerBase<EstimatorContext, InviteSendCommand, CompleteModel>
{
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IBaseAddressResolver _baseAddressResolver;
    private readonly IOptions<EnvironmentOptions> _environmentOptions;
    private readonly IOptions<EmailConfiguration> _emailOptions;

    public InviteSendHandler(
        ILoggerFactory loggerFactory,
        EstimatorContext dataContext,
        IMapper mapper,
        IEmailTemplateService emailTemplateService,
        IBaseAddressResolver baseAddressResolver,
        IOptions<EnvironmentOptions> environmentOptions,
        IOptions<EmailConfiguration> emailOptions)
        : base(loggerFactory, dataContext, mapper)
    {
        _emailTemplateService = emailTemplateService;
        _baseAddressResolver = baseAddressResolver;
        _environmentOptions = environmentOptions;
        _emailOptions = emailOptions;
    }

    protected override async ValueTask<CompleteModel?> Process(InviteSendCommand request, CancellationToken cancellationToken)
    {
        var invite = await DataContext.Invites
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (invite == null)
        {
            Logger.LogWarning("Invite not found for ID: {Id}", request.Id);
            return CompleteModel.Fail("Invite not found.");
        }

        var baseAddress = request.BaseAddress ?? _baseAddressResolver.GetBaseAddress();
        if (baseAddress.IsNullOrEmpty())
        {
            Logger.LogWarning("Base address is not configured for sending invite email.");
            return CompleteModel.Fail("Error: Base address is not configured.");
        }

        var fromName = _emailOptions.Value.FromName ?? "EstimatorX";
        var fromEmail = _emailOptions.Value.FromAddress;
        if (fromEmail.IsNullOrEmpty())
        {
            Logger.LogWarning("Email sender is not configured.");
            return CompleteModel.Fail("Error: Email sender is not configured.");
        }

        var companyName = _environmentOptions.Value.CompanyName;
        var productName = _environmentOptions.Value.ProjectName ?? "EstimatorX";

        // ensure security key is set
        var code = TokenService.GenerateCode();

        var inviteUrl = RouteLinks.Invites.Redeem(code);
        var linkUrl = baseAddress.Combine(inviteUrl);

        var inviteEmail = new InviteEmailModel
        {
            ExpireDays = 30,
            Link = linkUrl,
            ProductName = productName,
            CompanyName = companyName,
        };

        var builder = EmailBuilder.Create()
            .To(invite.Email, invite.Name)
            .From(fromEmail, fromName);

        var recipients = builder.BuildRecipients();
        var senders = builder.BuildSenders();

        var result = await _emailTemplateService.Send(
            templateName: TemplateNames.InviteTemplate,
            emailModel: inviteEmail,
            recipients: recipients,
            senders: senders,
            cancellationToken: cancellationToken);

        // store the hash of the code for security purposes, not the code itself
        invite.SecurityCode = TokenService.HashCode(code);

        // Update the invite model with the last sent time and expiration
        invite.LastSent = DateTimeOffset.UtcNow;
        invite.Expiration = DateTimeOffset.UtcNow.AddDays(30);
        invite.Updated = DateTimeOffset.UtcNow;
        invite.UpdatedBy = request.Principal?.GetName();

        await DataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // opportunistically remove expired invites
        var removed = await DataContext.Invites
            .RemoveExpiredInvites(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (removed > 0)
            Logger.LogInformation("Removed {Count} expired invites", removed);

        if (result.Successful)
            return CompleteModel.Success();

        var errorMessage = result.Message ?? result.Exception?.Message ?? "Unknown Error";
        Logger.LogError(result.Exception, "Failed to send invite email: {Error}", errorMessage);

        return CompleteModel.Fail(errorMessage);
    }
}
