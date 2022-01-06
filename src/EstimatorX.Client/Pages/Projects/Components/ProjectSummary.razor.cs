namespace EstimatorX.Client.Pages.Projects.Components;

public partial class ProjectSummary
{
    private Dictionary<string, bool> _collapseState = new Dictionary<string, bool>();

    private bool CollapseState(string id)
    {
        _collapseState.TryGetValue(id, out bool state);
        return state;
    }

    private bool ToggleCollapse(string id)
    {
        _collapseState.TryGetValue(id, out bool state);

        state = !state;
        _collapseState[id] = state;

        return state;
    }
}
