
using AutoMapper;

using Blazored.Modal.Services;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EstimatorX.Client.Pages.Templates.Components;

public partial class TemplateContainer<TStore, TRepository, TModel>
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : Project, IHaveIdentifier, new()
{
    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string OrganizationId { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string BodyClass { get; set; }


    [CascadingParameter]
    public IModalService Modal { get; set; }


    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    [Inject]
    public IMapper Mapper { get; set; }


    private EditContext EditContext { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // create edit context after project loaded
        if (Model != null && EditContext == null)
        {
            EditContext = new EditContext(Store.Model);
            EditContext.OnFieldChanged += HandleFormChange;
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        EditContext.OnFieldChanged -= HandleFormChange;
    }

    private async Task HandleSave()
    {
        try
        {
            await Store.Save(Id, OrganizationId);

            var model = Store.Model;

            NotificationService.ShowSuccess($"Template '{model.Name}' saved successfully");

            // route if org changed
            if (model.OrganizationId != OrganizationId)
                Navigation.NavigateTo($"/templates/{model.Id}/{model.OrganizationId}");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private async Task HandleDelete()
    {
        try
        {
            var name = Model.Name;

            if (!await Modal.ConfirmDelete($"Are you sure you want to delete template '{name}'?"))
                return;


            await Store.Delete(Id, OrganizationId);
            NotificationService.ShowSuccess($"Template '{name}' deleted successfully");
            Navigation.NavigateTo("/templates");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private async Task HandleDuplicate()
    {
        try
        {
            var name = Model.Name;

            var clone = Mapper.Map<TModel>(Model);
            clone.Name += " - Copy";

            var result = await Store.Repository.Create(clone);

            NotificationService.ShowSuccess($"Template '{name}' duplicated successfully");

            Navigation.NavigateTo($"/templates");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private void HandleFormChange(object sender, FieldChangedEventArgs args)
    {
        Store.NotifyStateChanged();
    }
}
