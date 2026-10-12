using Arbiter.CommandQuery.Commands;
using Arbiter.CommandQuery.Queries;
using Arbiter.Mapping;
using Arbiter.Mediation;

using EstimatorX.Data;
using EstimatorX.Domain.Commands;
using EstimatorX.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core.Tests.Domain;

[Collection(DatabaseCollection.CollectionName)]
public class UserOrganizationTests(DatabaseFixture fixture) : TestHostBase<DatabaseFixture>(fixture)
{
    [Fact]
    public async Task OwnerCommand_NewMembership_CreatesOwner()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var (userId, organizationId) = await CreateParents(mediator);

        var result = await mediator.Send(new UserOrganizationOwnerCommand(MockClaimsPrincipal.Default, userId, organizationId, true), TestCancellation);
        result.Should().NotBeNull();

        var entity = await FindMembership(userId, organizationId);
        entity.Should().NotBeNull();
        entity!.IsOwner.Should().BeTrue();
        entity.CreatedBy.Should().Be(MockClaimsPrincipal.Default.Identity?.Name);
        entity.UpdatedBy.Should().Be(MockClaimsPrincipal.Default.Identity?.Name);

        await DeleteParents(mediator, userId, organizationId);
    }

    [Fact]
    public async Task OwnerCommand_ExistingMembership_TogglesOwner()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var (userId, organizationId) = await CreateParents(mediator);

        var createCommand = new EntityCreateCommand<UserOrganizationCreateModel, UserOrganizationReadModel>(
            MockClaimsPrincipal.Default,
            new UserOrganizationCreateModel { UserId = userId, OrganizationId = organizationId });
        var createResult = await mediator.Send(createCommand, TestCancellation);
        createResult.Should().NotBeNull();

        await mediator.Send(new UserOrganizationOwnerCommand(MockClaimsPrincipal.Default, userId, organizationId, true), TestCancellation);

        var owner = await FindMembership(userId, organizationId);
        owner.Should().NotBeNull();
        owner!.Id.Should().Be(createResult.Id);
        owner.IsOwner.Should().BeTrue();

        await mediator.Send(new UserOrganizationOwnerCommand(MockClaimsPrincipal.Default, userId, organizationId, false), TestCancellation);

        var notOwner = await FindMembership(userId, organizationId);
        notOwner.Should().NotBeNull();
        notOwner!.Id.Should().Be(createResult.Id);
        notOwner.IsOwner.Should().BeFalse();

        await using (var scope = Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<EstimatorContext>();
            var count = await context.UserOrganizations.CountAsync(x => x.UserId == userId && x.OrganizationId == organizationId, TestCancellation);
            count.Should().Be(1);
        }

        await DeleteParents(mediator, userId, organizationId);
    }

    [Fact]
    public async Task DeleteCommand_ExistingMembership_RemovesMembership()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var (userId, organizationId) = await CreateParents(mediator);

        await mediator.Send(new UserOrganizationOwnerCommand(MockClaimsPrincipal.Default, userId, organizationId, false), TestCancellation);
        (await FindMembership(userId, organizationId)).Should().NotBeNull();

        var result = await mediator.Send(new UserOrganizationDeleteCommand(MockClaimsPrincipal.Default, userId, organizationId), TestCancellation);
        result.Should().NotBeNull();

        (await FindMembership(userId, organizationId)).Should().BeNull();

        await DeleteParents(mediator, userId, organizationId);
    }

    [Fact]
    public async Task DeleteCommand_MissingMembership_Succeeds()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var (userId, organizationId) = await CreateParents(mediator);

        var result = await mediator.Send(new UserOrganizationDeleteCommand(MockClaimsPrincipal.Default, userId, organizationId), TestCancellation);
        result.Should().NotBeNull();

        (await FindMembership(userId, organizationId)).Should().BeNull();

        await DeleteParents(mediator, userId, organizationId);
    }

    private async Task<(long UserId, long OrganizationId)> CreateParents(IMediator mediator)
    {
        var testValue = $"TEST-{DateTime.UtcNow.Ticks}";

        var userCommand = new EntityCreateCommand<UserCreateModel, UserReadModel>(
            MockClaimsPrincipal.Default,
            new UserCreateModel { Email = $"{Guid.NewGuid():N}@mailinator.com", Name = testValue });
        var userResult = await mediator.Send(userCommand, TestCancellation);
        userResult.Should().NotBeNull();

        var organizationCommand = new EntityCreateCommand<OrganizationCreateModel, OrganizationReadModel>(
            MockClaimsPrincipal.Default,
            new OrganizationCreateModel { Name = testValue });
        var organizationResult = await mediator.Send(organizationCommand, TestCancellation);
        organizationResult.Should().NotBeNull();

        return (userResult!.Id, organizationResult!.Id);
    }

    private async Task DeleteParents(IMediator mediator, long userId, long organizationId)
    {
        await mediator.Send(new UserOrganizationDeleteCommand(MockClaimsPrincipal.Default, userId, organizationId), TestCancellation);
        await mediator.Send(new EntityDeleteCommand<long, OrganizationReadModel>(MockClaimsPrincipal.Default, organizationId), TestCancellation);
        await mediator.Send(new EntityDeleteCommand<long, UserReadModel>(MockClaimsPrincipal.Default, userId), TestCancellation);
    }

    private async Task<EstimatorX.Data.Entities.UserOrganization?> FindMembership(long userId, long organizationId)
    {
        await using var scope = Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<EstimatorContext>();
        return await context.UserOrganizations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId, TestCancellation);
    }

    [Fact]
    public async Task EntityCommands_FullLifecycle_Succeeds()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        mediator.Should().NotBeNull();

        var mapper = Services.GetRequiredService<IMapper>();
        mapper.Should().NotBeNull();

        var userName = MockClaimsPrincipal.Default.Identity?.Name;
        userName.Should().NotBeNullOrEmpty();

        var testValue = $"TEST-{DateTime.UtcNow.Ticks}";

        // Create Parent User
        var userModel = new UserCreateModel
        {
            Email = $"{DateTime.Now.Ticks}@mailinator.com",
            Name = testValue,
        };
        var userCommand = new EntityCreateCommand<UserCreateModel, UserReadModel>(MockClaimsPrincipal.Default, userModel);
        var userResult = await mediator.Send(userCommand, TestCancellation);
        userResult.Should().NotBeNull();

        // Create Parent Organizations
        var tenantCommand = new EntityCreateCommand<OrganizationCreateModel, OrganizationReadModel>(MockClaimsPrincipal.Default, new OrganizationCreateModel { Name = testValue });
        var tenantResult = await mediator.Send(tenantCommand, TestCancellation);
        tenantResult.Should().NotBeNull();

        var updateOrganizationCommand = new EntityCreateCommand<OrganizationCreateModel, OrganizationReadModel>(MockClaimsPrincipal.Default, new OrganizationCreateModel { Name = $"{testValue}-UPDATE" });
        var updateOrganizationResult = await mediator.Send(updateOrganizationCommand, TestCancellation);
        updateOrganizationResult.Should().NotBeNull();

        // Create Entity
        var createModel = new UserOrganizationCreateModel
        {
            UserId = userResult.Id,
            OrganizationId = tenantResult.Id,
        };

        var createCommand = new EntityCreateCommand<UserOrganizationCreateModel, UserOrganizationReadModel>(MockClaimsPrincipal.Default, createModel);
        var createResult = await mediator.Send(createCommand, TestCancellation);
        createResult.Should().NotBeNull();
        createResult.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.CreatedBy.Should().Be(userName);
        createResult.Updated.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.UpdatedBy.Should().Be(userName);

        // Get Entity by Key
        var identifierQuery = new EntityIdentifierQuery<long, UserOrganizationReadModel>(MockClaimsPrincipal.Default, createResult.Id);
        var identifierResult = await mediator.Send(identifierQuery, TestCancellation);
        identifierResult.Should().NotBeNull();
        identifierResult.OrganizationId.Should().Be(createModel.OrganizationId);

        // Query Entity
        var entityQuery = new EntityQuery
        {
            Sort = [new EntitySort { Name = "Updated", Direction = SortDirections.Descending }],
            Filter = new EntityFilter { Name = nameof(UserOrganizationReadModel.UserId), Value = userResult.Id }
        };
        var listQuery = new EntityPagedQuery<UserOrganizationReadModel>(MockClaimsPrincipal.Default, entityQuery);

        var listResult = await mediator.Send(listQuery, TestCancellation);
        listResult.Should().NotBeNull();

        // Update Entity
        var updateModel = mapper.Map<UserOrganizationReadModel, UserOrganizationUpdateModel>(identifierResult);
        updateModel.OrganizationId = updateOrganizationResult.Id;

        var updateCommand = new EntityUpdateCommand<long, UserOrganizationUpdateModel, UserOrganizationReadModel>(MockClaimsPrincipal.Default, createResult.Id, updateModel);
        var updateResult = await mediator.Send(updateCommand, TestCancellation);
        updateResult.Should().NotBeNull();
        updateResult.Created.Should().Be(createResult.Created);
        updateResult.CreatedBy.Should().Be(userName);
        updateResult.Updated.Should().BeOnOrAfter(createResult.Updated);
        updateResult.UpdatedBy.Should().Be(userName);
        updateResult.OrganizationId.Should().Be(updateOrganizationResult.Id);

        // Delete Entity
        var deleteCommand = new EntityDeleteCommand<long, UserOrganizationReadModel>(MockClaimsPrincipal.Default, createResult.Id);
        var deleteResult = await mediator.Send(deleteCommand, TestCancellation);
        deleteResult.Should().NotBeNull();
        deleteResult.Id.Should().Be(createResult.Id);

        // Delete Parents
        await mediator.Send(new EntityDeleteCommand<long, OrganizationReadModel>(MockClaimsPrincipal.Default, updateOrganizationResult.Id), TestCancellation);
        await mediator.Send(new EntityDeleteCommand<long, OrganizationReadModel>(MockClaimsPrincipal.Default, tenantResult.Id), TestCancellation);
        await mediator.Send(new EntityDeleteCommand<long, UserReadModel>(MockClaimsPrincipal.Default, userResult.Id), TestCancellation);
    }
}
