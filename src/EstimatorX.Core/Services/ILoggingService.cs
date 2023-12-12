using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;
public interface ILoggingService
{
    Task<LogEventResult> Search(LogEventRequest request, CancellationToken cancellationToken);
}