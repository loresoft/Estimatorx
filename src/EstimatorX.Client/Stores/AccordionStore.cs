namespace EstimatorX.Client.Stores;

[RegisterScoped]
public class AccordionStore
{
    public event Action<string, Guid, bool> OnChange;

    public virtual void NotifyStateChanged(string parentGroup, Guid accordionId, bool collapsed)
    {
        OnChange?.Invoke(parentGroup, accordionId, collapsed);
    }
}
