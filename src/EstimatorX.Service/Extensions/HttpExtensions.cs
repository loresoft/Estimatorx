using EstimatorX.Shared.Models;

using UAParser;

namespace EstimatorX.Service.Extensions;

public static class HttpExtensions
{
    public static T GetBrowserData<T>(this HttpRequest httpRequest)
        where T : BrowserDetail, new()
    {
        var model = new T { Created = DateTimeOffset.UtcNow };

        if (httpRequest == null)
            return model;

        model.IpAddress = httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();
        model.UserAgent = httpRequest.Headers["User-Agent"].ToString();

        if (string.IsNullOrEmpty(model.UserAgent))
            return model;

        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(model.UserAgent);

        model.Browser = clientInfo.UA.ToString();
        model.OperatingSystem = clientInfo.OS.ToString();

        model.DeviceBrand = clientInfo.Device.Brand;
        model.DeviceFamily = clientInfo.Device.Family;
        model.DeviceModel = clientInfo.Device.Model;

        return model;
    }
}
