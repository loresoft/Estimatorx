using Blazored.Modal;
using Blazored.Modal.Services;

using EstimatorX.Shared.Definitions;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Components;

public partial class ReorderModal<TItem>
    where TItem : class, IHaveName
{
    [CascadingParameter]
    BlazoredModalInstance Modal { get; set; }

    [Parameter]
    public List<TItem> Items { get; set; }

    private async Task Close() => await Modal.CloseAsync(ModalResult.Ok(true));

    private async Task Cancel() => await Modal.CancelAsync();


    private TItem DragItem { get; set; }

    private void HandleDrop(TItem item)
    {
        if (DragItem == null || DragItem == item)
            return;

        var newIndex = Items.IndexOf(item);
        var oldIndex = Items.IndexOf(DragItem);

        Items.RemoveAt(oldIndex);
        Items.Insert(newIndex, DragItem);

        StateHasChanged();
    }

    private void HandleDragStart(TItem item)
    {
        DragItem = item;
    }

    private void HandleDragOver(TItem item)
    {
        // need to allow drop on item, don't remove
    }
}
