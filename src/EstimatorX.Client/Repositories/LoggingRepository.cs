using EstimatorX.Client.Services;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using FluentRest;

namespace EstimatorX.Client.Repositories;

[RegisterScoped]
public class LoggingRepository
{
    protected GatewayClient Gateway { get; }

    public LoggingRepository(GatewayClient gateway)
    {
        Gateway = gateway;
    }

    public async Task<LogEventResult> Search(LogEventRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        var result = await Gateway.GetAsync<LogEventResult>(b => b
            .AppendPath("/api/administrative/logging")
            .QueryStringIf(() => request.ContinuationToken.HasValue(), nameof(LogEventRequest.ContinuationToken), request.ContinuationToken)
            .QueryStringIf(() => request.PageSize != 100, nameof(LogEventRequest.PageSize), request.PageSize)
            .QueryStringIf(request.Level.HasValue, nameof(LogEventRequest.Level), request.Level)
            .QueryStringIf(() => request.Date != DateOnly.FromDateTime(DateTime.Now), nameof(LogEventRequest.Date), request.Date)
        );

        return result;
    }

}
