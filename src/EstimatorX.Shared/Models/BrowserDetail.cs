namespace EstimatorX.Shared.Models;

[Equatable]
public partial class BrowserDetail
{
    public string UserAgent { get; set; }

    public string Browser { get; set; }
    public string OperatingSystem { get; set; }

    public string DeviceFamily { get; set; }
    public string DeviceBrand { get; set; }
    public string DeviceModel { get; set; }

    public string IpAddress { get; set; }

    public DateTimeOffset? Created { get; set; }
}
