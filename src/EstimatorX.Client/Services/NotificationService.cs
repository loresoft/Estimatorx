using LoreSoft.Blazor.Controls;

namespace EstimatorX.Client.Services;

[RegisterSingleton]
public class NotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IToaster _toaster;

    public NotificationService(ILogger<NotificationService> logger, IToaster toaster)
    {
        _logger = logger;
        _toaster = toaster;
    }

    public void Clear()
    {
        _toaster.Clear();
    }

    public void ShowInformation(string message)
    {
        _toaster.ShowInformation(message);
    }

    public void ShowSuccess(string message)
    {
        _toaster.ShowInformation(message);
    }

    public void ShowWarning(string message)
    {
        _logger.LogWarning("Warning: {message}", message);
        _toaster.ShowWarning(message, config => config.Timeout = 15);
    }

    public void ShowWarning(Exception exception)
    {
        _logger.LogWarning(exception, "Warning: {message}", exception.Message);
        _toaster.ShowWarning(exception.Message, config => config.Timeout = 15);
    }

    public void ShowError(string message)
    {
        _logger.LogError("Error: {message}", message);
        _toaster.ShowError(message, config => config.Timeout = 30);
    }

    public void ShowError(Exception exception)
    {
        _logger.LogError(exception, "Error: {message}", exception.Message);
        _toaster.ShowError(exception.Message, config => config.Timeout = 30);
    }
}
