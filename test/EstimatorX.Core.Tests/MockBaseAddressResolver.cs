using Arbiter.CommandQuery.Definitions;
using Arbiter.CommandQuery.Extensions;

namespace EstimatorX.Core.Tests;

public sealed class MockBaseAddressResolver : IBaseAddressResolver
{
    public const string BaseAddress = "https://localhost";

    public string GetBaseAddress(string? path = null)
        => path is null ? BaseAddress : BaseAddress.Combine(path);
}
