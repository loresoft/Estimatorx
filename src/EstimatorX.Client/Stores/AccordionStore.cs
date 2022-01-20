using EstimatorX.Shared.Definitions;

namespace EstimatorX.Client.Stores;

public class AccordionStore : IServiceScoped
{
    public event Action<string, Guid, bool> OnChange;

    public virtual void NotifyStateChanged(string parentGroup, Guid accordionId, bool collapsed)
    {
        OnChange?.Invoke(parentGroup, accordionId, collapsed);
    }
}
