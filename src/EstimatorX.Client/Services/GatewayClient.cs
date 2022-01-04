
using FluentRest;

namespace EstimatorX.Client.Services;

public class GatewayClient : FluentClient
{
    public GatewayClient(HttpClient httpClient, IContentSerializer contentSerializer)
        : base(httpClient, contentSerializer)
    {
    }
}
