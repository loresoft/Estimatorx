
using Microsoft.JSInterop;

namespace EstimatorX.Client.Extensions;

public static class RuntimeExtensions
{
    public static ValueTask<bool> Confirm(this IJSRuntime jsRuntime, string message)
    {
        return jsRuntime.InvokeAsync<bool>("confirm", message);
    }
}
