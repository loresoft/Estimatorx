using Arbiter.CommandQuery.Commands;
using Arbiter.CommandQuery.Queries;
using Arbiter.Mapping;
using System.Text.RegularExpressions;

using Arbiter.Communication.Email;
using Arbiter.Mediation;
using Arbiter.Services;

using EstimatorX.Data;
using EstimatorX.Data.Entities;
using EstimatorX.Domain.Commands;
using EstimatorX.Domain.Constants;
using EstimatorX.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core.Tests.Domain;

[Collection(DatabaseCollection.CollectionName)]
public class InviteTests(DatabaseFixture fixture) : TestHostBase<DatabaseFixture>(fixture)
{
    [Fact]
    public async Task EntityCommands_FullLifecycle_Succeeds()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        mediator.Should().NotBeNull();

        var mapper = Services.GetRequiredService<IMapper>();
        mapper.Should().NotBeNull();

        var testValue = $"TEST-{DateTime.UtcNow.Ticks}";
        var userName = MockClaimsPrincipal.Default.Identity?.Name;
        userName.Should().NotBeNullOrEmpty();

        // Create Entity
        var createModel = new InviteCreateModel
        {
            Email = $"{DateTime.Now.Ticks}@mailinator.com",
            Name = testValue,
            OrganizationId = Organizations.Testing,
            SecurityCode = Guid.NewGuid().ToString("N"),
            Expiration = DateTime.UtcNow.AddDays(10)
        };

        var createCommand = new EntityCreateCommand<InviteCreateModel, InviteReadModel>(MockClaimsPrincipal.Default, createModel);
        var createResult = await mediator.Send(createCommand, TestCancellation);
        createResult.Should().NotBeNull();
        createResult.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.CreatedBy.Should().Be(userName);
        createResult.Updated.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.UpdatedBy.Should().Be(userName);

        // Get Entity by Key
        var identifierQuery = new EntityIdentifierQuery<long, InviteReadModel>(MockClaimsPrincipal.Default, createResult.Id);
        var identifierResult = await mediator.Send(identifierQuery, TestCancellation);
        identifierResult.Should().NotBeNull();
        identifierResult.Name.Should().Be(createModel.Name);

        // Query Entity
        var entityQuery = new EntityQuery
        {
            Sort = [new EntitySort { Name = "Updated", Direction = SortDirections.Descending }],
            Filter = new EntityFilter { Name = nameof(InviteReadModel.Name), Value = testValue }
        };
        var listQuery = new EntityPagedQuery<InviteReadModel>(MockClaimsPrincipal.Default, entityQuery);

        var listResult = await mediator.Send(listQuery, TestCancellation);
        listResult.Should().NotBeNull();

        // Update Entity
        var updateModel = mapper.Map<InviteReadModel, InviteUpdateModel>(identifierResult);
        updateModel.Name = "Update Command";

        var updateCommand = new EntityUpdateCommand<long, InviteUpdateModel, InviteReadModel>(MockClaimsPrincipal.Default, createResult.Id, updateModel);
        var updateResult = await mediator.Send(updateCommand, TestCancellation);
        updateResult.Should().NotBeNull();
        updateResult.Name.Should().Be("Update Command");
        updateResult.Created.Should().Be(createResult.Created);
        updateResult.CreatedBy.Should().Be(userName);
        updateResult.Updated.Should().BeOnOrAfter(createResult.Updated);
        updateResult.UpdatedBy.Should().Be(userName);

        // Delete Entity
        var deleteCommand = new EntityDeleteCommand<long, InviteReadModel>(MockClaimsPrincipal.Default, createResult.Id);
        var deleteResult = await mediator.Send(deleteCommand, TestCancellation);
        deleteResult.Should().NotBeNull();
        deleteResult.Id.Should().Be(createResult.Id);
    }

    [Fact]
    public async Task Send_ValidInvite_SendsEmailAndUpdatesInvite()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var email = CreateEmail();
        var inviteId = await CreateInvite(email);

        var sendCommand = new InviteSendCommand(MockClaimsPrincipal.Default, inviteId, BaseAddress);
        var sendResult = await mediator.Send(sendCommand, TestCancellation);

        sendResult.Should().NotBeNull();
        sendResult.Successful.Should().BeTrue();

        var token = GetSentToken(email);
        token.Should().NotBeNullOrEmpty();

        using var scope = Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var invite = await dataContext.Invites
            .AsNoTracking()
            .FirstAsync(p => p.Id == inviteId, TestCancellation);

        invite.SecurityCode.Should().Be(TokenService.HashCode(token));
        invite.LastSent.Should().NotBeNull();
        invite.Expiration.Should().BeCloseTo(invite.LastSent.Value.AddDays(30), TimeSpan.FromMinutes(1));
        invite.UpdatedBy.Should().Be(MockClaimsPrincipal.Default.Identity?.Name);
    }

    [Fact]
    public async Task Send_InviteNotFound_Fails()
    {
        var mediator = Services.GetRequiredService<IMediator>();

        var sendCommand = new InviteSendCommand(MockClaimsPrincipal.Default, -1, BaseAddress);
        var sendResult = await mediator.Send(sendCommand, TestCancellation);

        sendResult.Should().NotBeNull();
        sendResult.Successful.Should().BeFalse();
    }

    [Fact]
    public async Task Redeem_NewUser_CreatesUserAndOrganizationAndRemovesInvite()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var email = CreateEmail();
        var inviteId = await CreateInvite(email);

        InviteSendCommand sendRequest = new(MockClaimsPrincipal.Default, inviteId, BaseAddress);
        var sendResult = await mediator.Send(sendRequest, TestCancellation);
        sendResult!.Successful.Should().BeTrue();

        var token = GetSentToken(email);

        InviteRedeemCommand redeemRequest = new(token);
        var redeemResult = await mediator.Send(redeemRequest, TestCancellation);
        redeemResult.Should().NotBeNull();
        redeemResult.Successful.Should().BeTrue();

        using var scope = Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var user = await dataContext.Users
            .AsNoTracking()
            .Include(p => p.UserOrganizations)
            .FirstOrDefaultAsync(p => p.Email == email, TestCancellation);

        user.Should().NotBeNull();
        user.UserOrganizations.Should().Contain(p => p.OrganizationId == Organizations.Testing);

        var inviteExists = await dataContext.Invites.AnyAsync(p => p.Id == inviteId, TestCancellation);
        inviteExists.Should().BeFalse();
    }

    [Fact]
    public async Task Redeem_ExistingUser_AddsOrganizationWithoutDuplicateUser()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var email = CreateEmail();

        using (var seedScope = Services.CreateScope())
        {
            var seedContext = seedScope.ServiceProvider.GetRequiredService<EstimatorContext>();
            User user = new()
            {
                Email = email,
                Name = "Existing User",
                CreatedBy = "test",
                UpdatedBy = "test",
            };
            seedContext.Users.Add(user);
            await seedContext.SaveChangesAsync(TestCancellation);
        }

        var inviteId = await CreateInvite(email);

        InviteSendCommand sendRequest = new(MockClaimsPrincipal.Default, inviteId, BaseAddress);
        var sendResult = await mediator.Send(sendRequest, TestCancellation);
        sendResult!.Successful.Should().BeTrue();

        var token = GetSentToken(email);

        InviteRedeemCommand redeemRequest = new(token);
        var redeemResult = await mediator.Send(redeemRequest, TestCancellation);
        redeemResult.Should().NotBeNull();
        redeemResult.Successful.Should().BeTrue();

        using var scope = Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var users = await dataContext.Users
            .AsNoTracking()
            .Include(p => p.UserOrganizations)
            .Where(p => p.Email == email)
            .ToListAsync(TestCancellation);

        users.Should().ContainSingle();
        users[0].UserOrganizations.Should().ContainSingle(p => p.OrganizationId == Organizations.Testing);

        var inviteExists = await dataContext.Invites.AnyAsync(p => p.Id == inviteId, TestCancellation);
        inviteExists.Should().BeFalse();
    }

    [Fact]
    public async Task Redeem_AuthenticatedUser_DifferentEmail_AddsOrganizationToAuthenticatedUser()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var inviteEmail = CreateEmail();
        var userEmail = CreateEmail();

        long userId;
        using (var seedScope = Services.CreateScope())
        {
            var seedContext = seedScope.ServiceProvider.GetRequiredService<EstimatorContext>();
            User user = new()
            {
                Email = userEmail,
                Name = "Authenticated User",
                CreatedBy = "test",
                UpdatedBy = "test",
            };
            seedContext.Users.Add(user);
            await seedContext.SaveChangesAsync(TestCancellation);
            userId = user.Id;
        }

        var inviteId = await CreateInvite(inviteEmail);

        InviteSendCommand sendRequest = new(MockClaimsPrincipal.Default, inviteId, BaseAddress);
        var sendResult = await mediator.Send(sendRequest, TestCancellation);
        sendResult!.Successful.Should().BeTrue();

        var token = GetSentToken(inviteEmail);

        var principal = MockClaimsPrincipal.CreatePrincipal(userEmail, "Authenticated User");
        ((System.Security.Claims.ClaimsIdentity)principal.Identity!).AddClaim(
            new System.Security.Claims.Claim(Arbiter.CommandQuery.Services.ClaimNames.UserId, userId.ToString()));

        InviteRedeemCommand redeemRequest = new(principal, token);
        var redeemResult = await mediator.Send(redeemRequest, TestCancellation);
        redeemResult.Should().NotBeNull();
        redeemResult.Successful.Should().BeTrue();

        using var scope = Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var authenticatedUser = await dataContext.Users
            .AsNoTracking()
            .Include(p => p.UserOrganizations)
            .FirstAsync(p => p.Id == userId, TestCancellation);

        authenticatedUser.UserOrganizations.Should().ContainSingle(p => p.OrganizationId == Organizations.Testing);

        var inviteUserExists = await dataContext.Users.AnyAsync(p => p.Email == inviteEmail, TestCancellation);
        inviteUserExists.Should().BeFalse();
    }

    [Fact]
    public async Task Redeem_InvalidToken_Fails()
    {
        var mediator = Services.GetRequiredService<IMediator>();

        InviteRedeemCommand redeemRequest = new(Guid.NewGuid().ToString("N"));
        var redeemResult = await mediator.Send(redeemRequest, TestCancellation);

        redeemResult.Should().NotBeNull();
        redeemResult.Successful.Should().BeFalse();
    }

    [Fact]
    public async Task Redeem_ExpiredInvite_Fails()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var email = CreateEmail();
        var inviteId = await CreateInvite(email);

        InviteSendCommand sendRequest = new(MockClaimsPrincipal.Default, inviteId, BaseAddress);
        var sendResult = await mediator.Send(sendRequest, TestCancellation);
        sendResult!.Successful.Should().BeTrue();

        var token = GetSentToken(email);

        using (var expireScope = Services.CreateScope())
        {
            var expireContext = expireScope.ServiceProvider.GetRequiredService<EstimatorContext>();
            var invite = await expireContext.Invites.FirstAsync(p => p.Id == inviteId, TestCancellation);
            invite.Expiration = DateTimeOffset.UtcNow.AddDays(-1);
            await expireContext.SaveChangesAsync(TestCancellation);
        }

        InviteRedeemCommand redeemRequest = new(token);
        var redeemResult = await mediator.Send(redeemRequest, TestCancellation);
        redeemResult.Should().NotBeNull();
        redeemResult.Successful.Should().BeFalse();

        using var scope = Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<EstimatorContext>();

        var inviteExists = await dataContext.Invites.AnyAsync(p => p.Id == inviteId, TestCancellation);
        inviteExists.Should().BeTrue();
    }


    private const string BaseAddress = "https://localhost";

    private static string CreateEmail() => $"{Guid.NewGuid():N}@mailinator.com";

    private async Task<long> CreateInvite(string email, DateTimeOffset? expiration = null)
    {
        var mediator = Services.GetRequiredService<IMediator>();

        var createModel = new InviteCreateModel
        {
            Email = email,
            Name = $"TEST-{DateTime.UtcNow.Ticks}",
            OrganizationId = Organizations.Testing,
            SecurityCode = Guid.NewGuid().ToString("N"),
            Expiration = expiration ?? DateTimeOffset.UtcNow.AddDays(10)
        };

        var createCommand = new EntityCreateCommand<InviteCreateModel, InviteReadModel>(MockClaimsPrincipal.Default, createModel);
        var createResult = await mediator.Send(createCommand, TestCancellation);
        createResult.Should().NotBeNull();

        return createResult.Id;
    }

    private string GetSentToken(string email)
    {
        var deliveryService = Services.GetRequiredService<MemoryEmailDeliverService>();

        var message = deliveryService.Messages
            .LastOrDefault(m => m.Recipients.To.Any(t => string.Equals(t.Address, email, StringComparison.OrdinalIgnoreCase)));

        message.Should().NotBeNull($"an invite email should have been sent to {email}");

        var body = message!.Content.HtmlBody ?? message.Content.TextBody ?? string.Empty;
        var match = Regex.Match(body, @"/redeem/([^""'\s<&]+)");
        match.Success.Should().BeTrue("the invite email should contain a redeem link");

        return Uri.UnescapeDataString(match.Groups[1].Value);
    }
}
