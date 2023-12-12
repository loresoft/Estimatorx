using Azure.Data.Tables;

using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

[RegisterSingleton]
public class LoggingService : ILoggingService
{
    private readonly ILogger<LoggingService> _logger;
    private readonly TableServiceClient _tableServiceClient;
    private readonly Lazy<TableClient> _tableClient;

    public LoggingService(ILogger<LoggingService> logger, TableServiceClient tableServiceClient)
    {
        _logger = logger;
        _tableServiceClient = tableServiceClient;
        _tableClient = new Lazy<TableClient>(() => _tableServiceClient.GetTableClient("LogEvent"));
    }

    public async Task<LogEventResult> Search(LogEventRequest request, CancellationToken cancellationToken)
    {
        int pageSize = request.PageSize == 0 ? 100 : request.PageSize;

        var dateTime = request.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var upper = $"{DateTime.MaxValue.Ticks - dateTime.Ticks:D19}";
        var lower = $"{DateTime.MaxValue.Ticks - dateTime.AddDays(1).Ticks:D19}";

        var filter = $"(PartitionKey ge '{lower}') and (PartitionKey lt '{upper}')";

        if (request.Level.HasValue())
            filter += $" and (Level eq '{request.Level}')";

        var resultPageable = _tableClient.Value.QueryAsync<TableEntity>(filter, cancellationToken: cancellationToken);

        var resultPage = await resultPageable
            .AsPages(continuationToken: request.ContinuationToken, pageSizeHint: pageSize)
            .FirstOrDefaultAsync();

        var resultData = resultPage.Values
            .Select(entity => new LogEvent()
            {
                RowKey = entity.RowKey,
                PartitionKey = entity.PartitionKey,
                Timestamp = entity.Timestamp,
                ETag = entity.ETag.ToString(),
                Level = entity.GetString(nameof(LogEvent.Level)),
                MessageTemplate = entity.GetString(nameof(LogEvent.MessageTemplate)),
                RenderedMessage = entity.GetString(nameof(LogEvent.RenderedMessage)),
                Exception = entity.GetString(nameof(LogEvent.Exception)),
                Data = entity.GetString(nameof(LogEvent.Data)),
            })
            .OrderByDescending(p => p.Timestamp)
            .ToList();

        return new LogEventResult
        {
            ContinuationToken = resultPage.ContinuationToken,
            Data = resultData
        };
    }

}
