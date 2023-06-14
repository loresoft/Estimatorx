using Blazored.Modal;

using Blazored.Modal.Services;

using EstimatorX.Client.Components;
using EstimatorX.Shared.Definitions;

namespace EstimatorX.Client.Extensions;

public static class ModalExtensions
{
    public static async Task<bool> ConfirmDelete(this IModalService modalService, string message = null)
    {
        message ??= "Are you sure you want to delete this?";

        var parameters = new ModalParameters
        {
            { nameof(ConfirmModal.Message), message }
        };

        var messageForm = modalService.Show<ConfirmModal>("Confirm Delete", parameters);
        var result = await messageForm.Result;

        return !result.Cancelled;
    }

    public static async Task<bool> ConfirmNavigation(this IModalService modalService, string message = "Are you sure you want to navigate away? Changes you made will be lost.")
    {
        var parameters = new ModalParameters
        {
            { nameof(ConfirmModal.Message), message },
            { nameof(ConfirmModal.ActionName), "Yes" },
            { nameof(ConfirmModal.CancelName), "No" },
        };

        var messageForm = modalService.Show<ConfirmModal>("Confirm Navigation", parameters);
        var result = await messageForm.Result;

        return !result.Cancelled;
    }

    public static async Task<bool> Reorder<T>(this IModalService modalService, List<T> items)
        where T : class, IHaveName
    {
        var parameters = new ModalParameters
        {
            { nameof(ReorderModal<T>.Items), items }
        };

        var options = new ModalOptions();
        var messageForm = modalService.Show<ReorderModal<T>>("Reorder Items", parameters, options);
        var result = await messageForm.Result;

        return !result.Cancelled;
    }

}
