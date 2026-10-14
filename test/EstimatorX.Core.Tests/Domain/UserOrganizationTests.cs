using Arbiter.CommandQuery.Commands;
using Arbiter.CommandQuery.Queries;
using Arbiter.Mapping;
using Arbiter.Mediation;

using EstimatorX.Data;
using EstimatorX.Domain;
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

        UserOrganizationOwnerCommand request = new(MockClaimsPrincipal.Default, userId, organizationId, true);
        var result = await mediator.Send(request, TestCancellation);
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

        var createModel = new UserOrganizationCreateModel { UserId = userId, OrganizationId = organizationId };
        var createCommand = new EntityCreateCommand<UserOrganizationCreateModel, UserOrganizationReadModel>(
            MockClaimsPrincipal.Default,
            createModel,
            PipelineNames.System);

        var createResult = await mediator.Send(createCommand, TestCancellation);
        createResult.Should().NotBeNull();

        var ownerCommand = new UserOrganizationOwnerCommand(MockClaimsPrincipal.Default, userId, organizationId, true);
        await mediator.Send(ownerCommand, TestCancellation);

        var owner = await FindMembership(userId, organizationId);
        owner.Should().NotBeNull();
        owner!.Id.Should().Be(createResult.Id);
        owner.IsOwner.Should().BeTrue();

        var notOwnerCommand = new UserOrganizationOwnerCommand(MockClaimsPrincipal.Default, userId, organizationId, false);
        await mediator.Send(notOwnerCommand, TestCancellation);

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

        var ownerCommand = new UserOrganizationOwnerCommand(MockClaimsPrincipal.Default, userId, organizationId, false);
        await mediator.Send(ownerCommand, TestCancellation);

        var existing = await FindMembership(userId, organizationId);
        existing.Should().NotBeNull();

        var deleteCommand = new UserOrganizationDeleteCommand(MockClaimsPrincipal.Default, userId, organizationId);
        var result = await mediator.Send(deleteCommand, TestCancellation);
        result.Should().NotBeNull();

        var deleted = await FindMembership(userId, organizationId);
        deleted.Should().BeNull();

        await DeleteParents(mediator, userId, organizationId);
    }

    [Fact]
    public async Task DeleteCommand_MissingMembership_Succeeds()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        var (userId, organizationId) = await CreateParents(mediator);

        var deleteCommand = new UserOrganizationDeleteCommand(MockClaimsPrincipal.Default, userId, organizationId);
        var result = await mediator.Send(deleteCommand, TestCancellation);
        result.Should().NotBeNull();

        var membership = await FindMembership(userId, organizationId);
        membership.Should().BeNull();

        await DeleteParents(mediator, userId, organizationId);
    }

    private async Task<(long UserId, long OrganizationId)> CreateParents(IMediator mediator)
    {
        var testValue = $"TEST-{DateTime.UtcNow.Ticks}";

        var userModel = new UserCreateModel { Email = $"{Guid.NewGuid():N}@mailinator.com", Name = testValue };
        var userCommand = new EntityCreateCommand<UserCreateModel, UserReadModel>(
            MockClaimsPrincipal.Default,
            userModel,
            PipelineNames.System);

        var userResult = await mediator.Send(userCommand, TestCancellation);
        userResult.Should().NotBeNull();

        var organizationModel = new OrganizationCreateModel { Name = testValue };
        var organizationCommand = new EntityCreateCommand<OrganizationCreateModel, OrganizationReadModel>(
            MockClaimsPrincipal.Default,
            organizationModel,
            PipelineNames.System);

        var organizationResult = await mediator.Send(organizationCommand, TestCancellation);
        organizationResult.Should().NotBeNull();

        return (userResult!.Id, organizationResult!.Id);
    }

    private async Task DeleteParents(IMediator mediator, long userId, long organizationId)
    {
        var membershipCommand = new UserOrganizationDeleteCommand(MockClaimsPrincipal.Default, userId, organizationId);
        await mediator.Send(membershipCommand, TestCancellation);

        var organizationCommand = new EntityDeleteCommand<long, OrganizationReadModel>(MockClaimsPrincipal.Default, organizationId, PipelineNames.System);
        await mediator.Send(organizationCommand, TestCancellation);

        var userCommand = new EntityDeleteCommand<long, UserReadModel>(MockClaimsPrincipal.Default, userId, PipelineNames.System);
        await mediator.Send(userCommand, TestCancellation);
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
        var userCommand = new EntityCreateCommand<UserCreateModel, UserReadModel>(MockClaimsPrincipal.Default, userModel, PipelineNames.System);
        var userResult = await mediator.Send(userCommand, TestCancellation);
        userResult.Should().NotBeNull();

        // Create Parent Organizations
        var tenantModel = new OrganizationCreateModel { Name = testValue };
        var tenantCommand = new EntityCreateCommand<OrganizationCreateModel, OrganizationReadModel>(MockClaimsPrincipal.Default, tenantModel, PipelineNames.System);
        var tenantResult = await mediator.Send(tenantCommand, TestCancellation);
        tenantResult.Should().NotBeNull();

        var updateOrganizationModel = new OrganizationCreateModel { Name = $"{testValue}-UPDATE" };
        var updateOrganizationCommand = new EntityCreateCommand<OrganizationCreateModel, OrganizationReadModel>(MockClaimsPrincipal.Default, updateOrganizationModel, PipelineNames.System);
        var updateOrganizationResult = await mediator.Send(updateOrganizationCommand, TestCancellation);
        updateOrganizationResult.Should().NotBeNull();

        // Create Entity
        var createModel = new UserOrganizationCreateModel
        {
            UserId = userResult.Id,
            OrganizationId = tenantResult.Id,
        };

        var createCommand = new EntityCreateCommand<UserOrganizationCreateModel, UserOrganizationReadModel>(MockClaimsPrincipal.Default, createModel, PipelineNames.System);
        var createResult = await mediator.Send(createCommand, TestCancellation);
        createResult.Should().NotBeNull();
        createResult.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.CreatedBy.Should().Be(userName);
        createResult.Updated.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.UpdatedBy.Should().Be(userName);

        // Get Entity by Key
        var identifierQuery = new EntityIdentifierQuery<long, UserOrganizationReadModel>(MockClaimsPrincipal.Default, createResult.Id, PipelineNames.System);
        var identifierResult = await mediator.Send(identifierQuery, TestCancellation);
        identifierResult.Should().NotBeNull();
        identifierResult.OrganizationId.Should().Be(createModel.OrganizationId);

        // Query Entity
        var entityQuery = new EntityQuery
        {
            Sort = [new EntitySort { Name = "Updated", Direction = SortDirections.Descending }],
            Filter = new EntityFilter { Name = nameof(UserOrganizationReadModel.UserId), Value = userResult.Id }
        };
        var listQuery = new EntityPagedQuery<UserOrganizationReadModel>(MockClaimsPrincipal.Default, entityQuery, PipelineNames.System);

        var listResult = await mediator.Send(listQuery, TestCancellation);
        listResult.Should().NotBeNull();

        // Update Entity
        var updateModel = mapper.Map<UserOrganizationReadModel, UserOrganizationUpdateModel>(identifierResult);
        updateModel.OrganizationId = updateOrganizationResult.Id;

        var updateCommand = new EntityUpdateCommand<long, UserOrganizationUpdateModel, UserOrganizationReadModel>(MockClaimsPrincipal.Default, createResult.Id, updateModel, filterName: PipelineNames.System);
        var updateResult = await mediator.Send(updateCommand, TestCancellation);
        updateResult.Should().NotBeNull();
        updateResult.Created.Should().Be(createResult.Created);
        updateResult.CreatedBy.Should().Be(userName);
        updateResult.Updated.Should().BeOnOrAfter(createResult.Updated);
        updateResult.UpdatedBy.Should().Be(userName);
        updateResult.OrganizationId.Should().Be(updateOrganizationResult.Id);

        // Delete Entity
        var deleteCommand = new EntityDeleteCommand<long, UserOrganizationReadModel>(MockClaimsPrincipal.Default, createResult.Id, PipelineNames.System);
        var deleteResult = await mediator.Send(deleteCommand, TestCancellation);
        deleteResult.Should().NotBeNull();
        deleteResult.Id.Should().Be(createResult.Id);

        // Delete Parents
        var deleteUpdateOrganizationCommand = new EntityDeleteCommand<long, OrganizationReadModel>(MockClaimsPrincipal.Default, updateOrganizationResult.Id, PipelineNames.System);
        await mediator.Send(deleteUpdateOrganizationCommand, TestCancellation);

        var deleteTenantCommand = new EntityDeleteCommand<long, OrganizationReadModel>(MockClaimsPrincipal.Default, tenantResult.Id, PipelineNames.System);
        await mediator.Send(deleteTenantCommand, TestCancellation);

        var deleteUserCommand = new EntityDeleteCommand<long, UserReadModel>(MockClaimsPrincipal.Default, userResult.Id, PipelineNames.System);
        await mediator.Send(deleteUserCommand, TestCancellation);
    }
}
