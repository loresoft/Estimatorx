using EstimatorX.Domain.Definitions;
using EstimatorX.Domain.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Components;

/// <summary>
/// Shared state cascaded to estimate editor components.
/// </summary>
public class EstimateContext
{
    public long Id { get; set; }

    public IProjectEstimate Model { get; set; } = null!;

    public EventCallback StateChanged { get; set; }

    public ProjectSettings Settings => Model.Settings;

    public Task NotifyStateChanged() => StateChanged.InvokeAsync();
}
