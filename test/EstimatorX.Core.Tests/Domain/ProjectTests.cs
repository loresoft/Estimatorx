using Arbiter.CommandQuery.Commands;
using Arbiter.CommandQuery.Queries;
using Arbiter.Mapping;
using Arbiter.Mediation;

using EstimatorX.Domain.Constants;
using EstimatorX.Domain.Models;
using EstimatorX.Services;

using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core.Tests.Domain;

[Collection(DatabaseCollection.CollectionName)]
public class ProjectTests(DatabaseFixture fixture) : TestHostBase<DatabaseFixture>(fixture)
{
    [Fact]
    public async Task EntityCommands_FullLifecycle_Succeeds()
    {
        var mediator = Services.GetRequiredService<IMediator>();
        mediator.Should().NotBeNull();

        var mapper = Services.GetRequiredService<IMapper>();
        mapper.Should().NotBeNull();

        var builder = Services.GetRequiredService<IProjectBuilder>();
        builder.Should().NotBeNull();

        var calculator = Services.GetRequiredService<IProjectCalculator>();
        calculator.Should().NotBeNull();

        var userName = MockClaimsPrincipal.Default.Identity?.Name;
        userName.Should().NotBeNullOrEmpty();

        var testValue = $"TEST-{DateTime.UtcNow.Ticks}";

        // Create Entity
        var createModel = new ProjectCreateModel
        {
            Name = testValue,
            Description = "Test Project",
            OrganizationId = Organizations.Testing,

        };

        builder.UpdateProject(createModel, true);
        calculator.UpdateProject(createModel);

        var createCommand = new EntityCreateCommand<ProjectCreateModel, ProjectReadModel>(MockClaimsPrincipal.Default, createModel);
        var createResult = await mediator.Send(createCommand, TestCancellation);
        createResult.Should().NotBeNull();
        createResult.Created.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.CreatedBy.Should().Be(userName);
        createResult.Updated.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
        createResult.UpdatedBy.Should().Be(userName);

        // Get Entity by Key
        var identifierQuery = new EntityIdentifierQuery<long, ProjectReadModel>(MockClaimsPrincipal.Default, createResult.Id);
        var identifierResult = await mediator.Send(identifierQuery, TestCancellation);
        identifierResult.Should().NotBeNull();
        identifierResult.Name.Should().Be(createModel.Name);
        identifierResult.Settings.Should().NotBeNull();
        identifierResult.Epics.Should().NotBeNull();

        // Query Entity
        var entityQuery = new EntityQuery
        {
            Sort = [new EntitySort { Name = "Updated", Direction = SortDirections.Descending }],
            Filter = new EntityFilter { Name = nameof(ProjectReadModel.Name), Value = testValue }
        };
        var listQuery = new EntityPagedQuery<ProjectListModel>(MockClaimsPrincipal.Default, entityQuery);

        var listResult = await mediator.Send(listQuery, TestCancellation);
        listResult.Should().NotBeNull();

        // Update Entity
        var updateModel = mapper.Map<ProjectReadModel, ProjectUpdateModel>(identifierResult);
        updateModel.Name = "Update Command";

        var updateCommand = new EntityUpdateCommand<long, ProjectUpdateModel, ProjectReadModel>(MockClaimsPrincipal.Default, createResult.Id, updateModel);
        var updateResult = await mediator.Send(updateCommand, TestCancellation);
        updateResult.Should().NotBeNull();
        updateResult.Created.Should().Be(createResult.Created);
        updateResult.CreatedBy.Should().Be(userName);
        updateResult.Updated.Should().BeOnOrAfter(createResult.Updated);
        updateResult.UpdatedBy.Should().Be(userName);
        updateResult.Name.Should().Be("Update Command");

        // Delete Entity
        //var deleteCommand = new EntityDeleteCommand<long, ProjectReadModel>(MockClaimsPrincipal.Default, createResult.Id);
        //var deleteResult = await mediator.Send(deleteCommand, TestCancellation);
        //deleteResult.Should().NotBeNull();
        //deleteResult.Id.Should().Be(createResult.Id);
    }
}
