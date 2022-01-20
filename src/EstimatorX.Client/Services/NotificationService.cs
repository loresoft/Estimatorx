using EstimatorX.Shared.Definitions;

using Sotsera.Blazor.Toaster;

namespace EstimatorX.Client.Services;

public enum NotificationLevel
{
    Info,
    Success,
    Warning,
    Error
}

public record Notification(NotificationLevel Level, string Message);

public class NotificationService : IServiceScoped
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IToaster _toaster;


    public NotificationService(ILogger<NotificationService> logger, IToaster toaster)
    {
        _logger = logger;
        _toaster = toaster;
    }

    public event Action OnClear;
    public event Action<Notification> OnShow;

    public void Clear()
    {
        _toaster.Clear();
        OnClear?.Invoke();
    }

    public void ShowInfo(string message)
    {
        var notification = new Notification(NotificationLevel.Info, message);
        ShowToast(notification);
    }

    public void ShowSuccess(string message)
    {
        var notification = new Notification(NotificationLevel.Success, message);
        ShowToast(notification);
    }

    public void ShowWarning(string message)
    {
        _logger.LogWarning("Warning: {message}", message);

        var notification = new Notification(NotificationLevel.Warning, message);
        ShowToast(notification);
    }

    public void ShowWarning(Exception exception)
    {
        _logger.LogWarning(exception, "Warning: {message}", exception.Message);

        var notification = new Notification(NotificationLevel.Warning, exception.Message);
        ShowToast(notification);

    }

    public void ShowError(string message)
    {
        _logger.LogError("Error: {message}", message);

        var notification = new Notification(NotificationLevel.Error, message);
        ShowToast(notification);
    }

    public void ShowError(Exception exception)
    {
        _logger.LogError(exception, "Error: {message}", exception.Message);

        var notification = new Notification(NotificationLevel.Error, exception.Message);
        ShowToast(notification);
    }

    public void ShowToast(Notification notification)
    {
        switch (notification.Level)
        {
            case NotificationLevel.Info:
                _toaster.Info(notification.Message);
                break;
            case NotificationLevel.Success:
                _toaster.Success(notification.Message);
                break;
            case NotificationLevel.Warning:
                _toaster.Warning(notification.Message, configure: options => options.VisibleStateDuration = 20000);
                break;
            case NotificationLevel.Error:
                _toaster.Error(notification.Message, configure: options => options.VisibleStateDuration = 30000);
                break;
            default:
                _toaster.Info(notification.Message);
                break;
        }

        OnShow?.Invoke(notification);
    }
}
