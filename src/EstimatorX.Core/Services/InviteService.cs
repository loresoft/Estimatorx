
using System.Security.Policy;
using System.Security.Principal;

using AutoMapper;

using EstimatorX.Core.Options;
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using FluentRest;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace EstimatorX.Core.Services;

public class InviteService : OrganizationServiceBase<IInviteRepository, Invite>, IServiceTransient, IInviteService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly IOptions<HostingConfiguration> _hostingOptions;
    private readonly IOptions<SendGridConfiguration> _sendGridOptions;
    private readonly IOrganizationRepository _organizationRepository;

    public InviteService(
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IInviteRepository repository,
        IUserCache userCache,
        ISendGridClient sendGridClient,
        IOptions<HostingConfiguration> hostingOptions,
        IOptions<SendGridConfiguration> sendGridOptions,
        IOrganizationRepository organizationRepository)
        : base(loggerFactory, mapper, repository, userCache)
    {
        _sendGridClient = sendGridClient;
        _hostingOptions = hostingOptions;
        _sendGridOptions = sendGridOptions;
        _organizationRepository = organizationRepository;
    }

    public override async Task<Invite> Create(Invite model, IPrincipal principal, CancellationToken cancellationToken)
    {
        var result = await base.Create(model, principal, cancellationToken);

        await Send(result, principal, cancellationToken);

        return result;
    }

    public async Task<Invite> LoadBySecurityKey(string securityKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var entity = await Repository.FindOneAsync(i => i.SecurityKey == securityKey, cancellationToken: cancellationToken);
        if (entity == null)
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Invite not found");
                
        return entity;
    }


    public async Task<bool> Send(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var invite = await Repository.FindAsync(id, partitionKey, cancellationToken: cancellationToken);
        if (invite == null)
            throw new DomainException(System.Net.HttpStatusCode.InternalServerError, "Invite not found");

        return await Send(invite, principal, cancellationToken);
    }

    public async Task<bool> Redeem(string securityKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var invite = await Repository.FindOneAsync(i => i.SecurityKey == securityKey, cancellationToken: cancellationToken);
        if (invite == null)
            throw new DomainException(System.Net.HttpStatusCode.BadRequest, "Invalid invite code");

        var user = await CurrentUser(principal);
        if (user == null)
            throw new DomainException(System.Net.HttpStatusCode.BadRequest, "Invalid user");

        var organization = await _organizationRepository.FindAsync(invite.OrganizationId, cancellationToken: cancellationToken);
        if (user == null)
            throw new DomainException(System.Net.HttpStatusCode.BadRequest, "Invalid organization");

        // add user to org
        if (!organization.Members.Any(m => m.Id == user.Id))
        {
            var member = new OrganizationMember
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };
            organization.Members.Add(member);

            await _organizationRepository.SaveAsync(organization, cancellationToken);
        }

        // add org to user
        if (!user.Organizations.Any(o => o.Id == organization.Id))
        {
            var organizationName = new IdentifierName
            {
                Id = organization.Id,
                Name = organization.Name
            };
            user.Organizations.Add(organizationName);

            await UserCache.Repository.SaveAsync(user, cancellationToken);
            UserCache.Clear(user.Id);
        }

        // remove invite
        await Repository.DeleteAsync(invite, cancellationToken);

        return true;
    }


    protected override IQueryable<Invite> SearchQuery(IQueryable<Invite> query, string searchTerm)
    {
        if (searchTerm.IsNullOrWhiteSpace())
            return query;

        return query
            .Where(u => u.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));
    }


    private async Task<bool> Send(Invite invite, IPrincipal principal, CancellationToken cancellationToken)
    {
        if (!await HasAccess(principal, invite, cancellationToken))
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized to load this entity");

        Logger.LogInformation("Sending invite to: {email}; Organization: {organziation}", invite.Email, invite.OrganizationName);

        string senderName = principal.GetName();
        string senderEmail = principal.GetEmail();

        var link = new UrlBuilder(_hostingOptions.Value.ClientDomain)
            .AppendPath("invite")
            .AppendPath(invite.SecurityKey);

        string subject = $"Invite to the {invite.OrganizationName} organization on EstimatorX.com";
        string body = $"<p>{senderName} invited you to join the {invite.OrganizationName} organization on EstimatorX.com.</p>" +
                      "<p>EstimatorX is a simple project estimation application.</p>" +
                      $"<p><a href=\"{link}\">{link}</a></p>";

        var message = new SendGridMessage();
        message.From = new EmailAddress(_sendGridOptions.Value.FromEmail, _sendGridOptions.Value.FromName);

        if (senderEmail.HasValue())
            message.ReplyTo = new EmailAddress(senderEmail, senderName);

        message.AddTo(invite.Email, invite.Name);

        message.Subject = subject;
        message.AddContent(System.Net.Mime.MediaTypeNames.Text.Html, body);

        var response = await _sendGridClient.SendEmailAsync(message, cancellationToken);

        Logger.LogInformation("Sent invite to: {email}; Organization: {organziation}; Status: {httpStatus}", invite.Email, invite.OrganizationName, response.StatusCode);

        return response.IsSuccessStatusCode;
    }
}
