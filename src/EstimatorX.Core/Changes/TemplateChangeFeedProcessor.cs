using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Changes;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Changes;

[RegisterSingleton<IHostedService>(Duplicate = DuplicateStrategy.Append)]
public class TemplateChangeFeedProcessor : IHostedService, IAsyncDisposable
{
    private readonly ILogger _logger;
    private readonly ITemplateRepository _templateRepository;
    private readonly IHubContext<ChangeFeedHub> _changeFeedHub;

    private ChangeFeedProcessor _changeFeedProcessor;

    public TemplateChangeFeedProcessor(
        ILogger<TemplateChangeFeedProcessor> logger,
        ITemplateRepository templateRepository,
        IHubContext<ChangeFeedHub> changeFeedHub)
    {
        _logger = logger;
        _templateRepository = templateRepository;
        _changeFeedHub = changeFeedHub;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Template change feed processor starting");
        //await InitializeProcessor();

        //await _changeFeedProcessor.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_changeFeedProcessor == null)
            return;

        _logger.LogInformation("Template change feed processor stopping");
        await _changeFeedProcessor.StopAsync();
    }

    private async Task InitializeProcessor()
    {
        if (_changeFeedProcessor != null)
            return;

        _logger.LogInformation("Initialize Template change feed processor");

        var templateContainer = await _templateRepository.GetContainerAsync();

        var leaseContainerResponse = await templateContainer.Database
            .CreateContainerIfNotExistsAsync(
                new ContainerProperties(ChangeFeedConstants.LeaseContainer, ChangeFeedConstants.LeasePartitionKey),
                ThroughputProperties.CreateManualThroughput(400)
            );

        var leaseContainer = leaseContainerResponse.Container;

        _changeFeedProcessor = templateContainer
            .GetChangeFeedProcessorBuilder<Template>(nameof(TemplateChangeFeedProcessor), HandleChangesAsync)
            .WithInstanceName($"{nameof(Template)}-{Environment.MachineName}-{Environment.ProcessId}")
            .WithLeaseContainer(leaseContainer)
            .Build();
    }

    private async Task HandleChangesAsync(ChangeFeedProcessorContext context, IReadOnlyCollection<Template> changes, CancellationToken cancellationToken)
    {
        if (changes == null || changes.Count == 0)
            return;

        _logger.LogInformation("Template changes handled: {count}", changes.Count);

        foreach (var template in changes)
        {
            _logger.LogDebug("Template {TemplateId} {TemplateName} changed", template?.Id, template?.Name);
            await _changeFeedHub.Clients.All.SendAsync(ChangeFeedConstants.TemplateChangeEventName, template, cancellationToken: cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync(CancellationToken.None);
    }
}
