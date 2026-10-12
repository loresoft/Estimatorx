namespace EstimatorX.Services;

public enum AlertType
{
    Info,
    Success,
    Warning,
    Danger,
}

public sealed record AlertMessage(AlertType Type, string Text)
{
    public string CssClass => Type switch
    {
        AlertType.Success => "alert-success",
        AlertType.Warning => "alert-warning",
        AlertType.Danger => "alert-danger",
        _ => "alert-info",
    };

    public string Icon => Type switch
    {
        AlertType.Success => "bi-check-circle",
        AlertType.Warning => "bi-exclamation-circle",
        AlertType.Danger => "bi-exclamation-triangle",
        _ => "bi-info-circle",
    };
}

public static class AlertService
{
    public const string InviteAccepted = "invite-accepted";
    public const string LoginFailed = "login-failed";

    public static AlertMessage? GetMessage(string? flag)
    {
        return flag?.ToLowerInvariant() switch
        {
            InviteAccepted => new(AlertType.Success, "Your invite has been accepted. Sign in to continue."),
            LoginFailed => new(AlertType.Danger, "Sign in failed. Please try again."),
            _ => null,
        };
    }
}
