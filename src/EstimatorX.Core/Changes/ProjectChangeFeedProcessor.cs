using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Changes;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Changes;

[RegisterSingleton<IHostedService>(Duplicate = DuplicateStrategy.Append)]
public class ProjectChangeFeedProcessor : IHostedService, IAsyncDisposable
{
    private readonly ILogger _logger;
    private readonly IProjectRepository _projectRepository;
    private readonly IHubContext<ChangeFeedHub> _changeFeedHub;

    private ChangeFeedProcessor _changeFeedProcessor;

    public ProjectChangeFeedProcessor(
        ILogger<ProjectChangeFeedProcessor> logger,
        IProjectRepository projectRepository,
        IHubContext<ChangeFeedHub> changeFeedHub)
    {
        _logger = logger;
        _projectRepository = projectRepository;
        _changeFeedHub = changeFeedHub;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Project change feed processor starting");
        await InitializeProcessor();

        await _changeFeedProcessor.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Project change feed processor stopping");
        await _changeFeedProcessor.StopAsync();
    }

    private async Task InitializeProcessor()
    {
        if (_changeFeedProcessor != null)
            return;

        _logger.LogInformation("Initialize Project change feed processor");

        var projectContainer = await _projectRepository.GetContainerAsync();

        var leaseContainerResponse = await projectContainer.Database
            .CreateContainerIfNotExistsAsync(
                new ContainerProperties(ChangeFeedConstants.LeaseContainer, ChangeFeedConstants.LeasePartitionKey),
                ThroughputProperties.CreateManualThroughput(400)
            );

        var leaseContainer = leaseContainerResponse.Container;

        _changeFeedProcessor = projectContainer
            .GetChangeFeedProcessorBuilder<Project>(nameof(ProjectChangeFeedProcessor), HandleChangesAsync)
            .WithInstanceName($"{nameof(Project)}-{Environment.MachineName}-{Environment.ProcessId}")
            .WithLeaseContainer(leaseContainer)
            .Build();
    }

    private async Task HandleChangesAsync(ChangeFeedProcessorContext context, IReadOnlyCollection<Project> changes, CancellationToken cancellationToken)
    {
        if (changes == null || changes.Count == 0)
            return;

        _logger.LogInformation("Project changes handled: {count}", changes.Count);

        foreach (var project in changes)
        {
            _logger.LogDebug("Project {ProjectId} {ProjectName} changed", project?.Id, project?.Name);
            await _changeFeedHub.Clients.All.SendAsync(ChangeFeedConstants.ProjectChangeEventName, project, cancellationToken: cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync(CancellationToken.None);
    }
}
