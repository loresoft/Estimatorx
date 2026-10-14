using Arbiter.CommandQuery.Commands;
using Arbiter.CommandQuery.Queries;
using Arbiter.Mapping;
using Arbiter.Mediation;

using EstimatorX.Domain.Models;

using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core.Tests.Domain;

[Collection(DatabaseCollection.CollectionName)]
public class UserProviderTests(DatabaseFixture fixture) : TestHostBase<DatabaseFixture>(fixture)
{
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

        // Create Entity
        var createModel = new UserProviderCreateModel
        {
            Provider = testValue,
            ProviderKey = Guid.NewGuid().ToString("N"),
            UserId = userResult.Id,
        };

        var createCommand = new EntityCreateCommand<UserProviderCreateModel, UserProviderReadModel>(MockClaimsPrincipal.Default, createModel);
        var createResult = await mediator.Send(createCommand, TestCancellation);
        createResult.Should().NotBeNull();
        createResult.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.CreatedBy.Should().Be(userName);
        createResult.Updated.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.UpdatedBy.Should().Be(userName);

        // Get Entity by Key
        var identifierQuery = new EntityIdentifierQuery<long, UserProviderReadModel>(MockClaimsPrincipal.Default, createResult.Id);
        var identifierResult = await mediator.Send(identifierQuery, TestCancellation);
        identifierResult.Should().NotBeNull();
        identifierResult.Provider.Should().Be(createModel.Provider);

        // Query Entity
        var entityQuery = new EntityQuery
        {
            Sort = [new EntitySort { Name = "Updated", Direction = SortDirections.Descending }],
            Filter = new EntityFilter { Name = nameof(UserProviderReadModel.Provider), Value = testValue }
        };
        var listQuery = new EntityPagedQuery<UserProviderReadModel>(MockClaimsPrincipal.Default, entityQuery);

        var listResult = await mediator.Send(listQuery, TestCancellation);
        listResult.Should().NotBeNull();

        // Update Entity
        var updateModel = mapper.Map<UserProviderReadModel, UserProviderUpdateModel>(identifierResult);
        updateModel.Provider = "Update Command";

        var updateCommand = new EntityUpdateCommand<long, UserProviderUpdateModel, UserProviderReadModel>(MockClaimsPrincipal.Default, createResult.Id, updateModel);
        var updateResult = await mediator.Send(updateCommand, TestCancellation);
        updateResult.Should().NotBeNull();
        updateResult.Created.Should().Be(createResult.Created);
        updateResult.CreatedBy.Should().Be(userName);
        updateResult.Updated.Should().BeOnOrAfter(createResult.Updated);
        updateResult.UpdatedBy.Should().Be(userName);
        updateResult.Provider.Should().Be("Update Command");

        // Delete Entity
        var deleteCommand = new EntityDeleteCommand<long, UserProviderReadModel>(MockClaimsPrincipal.Default, createResult.Id);
        var deleteResult = await mediator.Send(deleteCommand, TestCancellation);
        deleteResult.Should().NotBeNull();
        deleteResult.Id.Should().Be(createResult.Id);

        // Delete Parent User
        var userDeleteCommand = new EntityDeleteCommand<long, UserReadModel>(MockClaimsPrincipal.Default, userResult.Id);
        var userDeleteResult = await mediator.Send(userDeleteCommand, TestCancellation);
        userDeleteResult.Should().NotBeNull();
    }
}
