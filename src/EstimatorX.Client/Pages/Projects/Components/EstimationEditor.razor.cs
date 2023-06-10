using AutoMapper;

using Blazored.Modal;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class EstimationEditor<TStore, TRepository, TModel>
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : Project, IHaveIdentifier, new()
{

    [Inject]
    public IMapper Mapper { get; set; }


    public ProjectSettings ProjectSettings => Store.Model.Settings;


    private string Identifier(params string[] names) => String.Join("-", names);


    private void EpicAdd()
    {
        Model.Epics.Add(new EpicEstimate { Name = "New Epic Estimate" });
        Store.NotifyStateChanged();
    }

    private async Task EpicReorder()
    {
        var result = await Modal.Reorder(Model.Epics);
        Store.NotifyStateChanged();
    }

    private async Task EpicDelete(EpicEstimate epic)
    {
        var name = epic.Name;

        if (!await Modal.ConfirmDelete($"Are you sure you want to delete epic '{name}'?"))
            return;


        Model.Epics.Remove(epic);

        Store.NotifyStateChanged();
    }

    private void EpicDuplicate(EpicEstimate epic)
    {
        var clone = Mapper.Map<EpicEstimate>(epic);

        clone.Id = Guid.NewGuid().ToString("N");
        clone.Name += " - Copy";

        Model.Epics.Add(clone);

        Store.NotifyStateChanged();
    }


    private void FeatureAdd(EpicEstimate epic)
    {
        epic.Features.Add(new FeatureEstimate { Name = "New Feature Estimate" });
        Store.NotifyStateChanged();
    }

    private async Task FeatureReporder(EpicEstimate epic)
    {
        var result = await Modal.Reorder(epic.Features);
        Store.NotifyStateChanged();
    }

    private async Task FeatureDelete(EpicEstimate epic, FeatureEstimate feature)
    {
        var name = feature.Name;

        if (!await Modal.ConfirmDelete($"Are you sure you want to delete feature '{name}'?"))
            return;

        epic.Features.Remove(feature);

        Store.NotifyStateChanged();
    }

    private void FeatureDuplicate(EpicEstimate epic, FeatureEstimate feature)
    {
        var clone = Mapper.Map<FeatureEstimate>(feature);

        clone.Id = Guid.NewGuid().ToString("N");
        clone.Name += " - Copy";

        epic.Features.Add(clone);

        Store.NotifyStateChanged();
    }

    private async Task FeatureMove(FeatureEstimate feature)
    {
        var name = feature.Name;

        var parameters = new ModalParameters();
        parameters.Add(nameof(FeatureMoveModal.Project), Store.Model);
        parameters.Add(nameof(FeatureMoveModal.Feature), feature);

        var messageForm = Modal.Show<FeatureMoveModal>("Move Feature", parameters);
        var result = await messageForm.Result;

        Store.NotifyStateChanged();
    }

}
