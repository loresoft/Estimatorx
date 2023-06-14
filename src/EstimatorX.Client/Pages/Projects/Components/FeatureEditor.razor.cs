using AutoMapper;

using Blazored.Modal;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureEditor<TStore, TRepository, TModel>
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : Project, IHaveIdentifier, new()
{
    [Parameter]
    public string ParentCollapse { get; set; }

    [Parameter]
    public EpicEstimate Epic { get; set; }

    [Parameter]
    public FeatureEstimate Feature { get; set; }

    [Inject]
    public IMapper Mapper { get; set; }

    public ProjectSettings ProjectSettings => Store.Model?.Settings;


    private string Identifier(string name) => $"feature-{name}-{Feature?.Id}";

    private double? OverheadEstimate(double multiplier, double? estimate)
    {
        var value = (estimate * multiplier) - estimate;
        return value.HasValue ? Math.Round(value.Value, 2) : null;
    }

    private double? OverheadAmmount(double multiplier, double? estimate)
    {
        var value = OverheadEstimate(multiplier, estimate);
        return value.HasValue ? value.Value * ProjectSettings.EstimateRate : null;
    }

    private async Task FeatureDelete()
    {
        var name = Feature.Name;

        if (!await Modal.ConfirmDelete($"Are you sure you want to delete feature '{name}'?"))
            return;

        Epic.Features.Remove(Feature);

        Store.NotifyStateChanged();
    }

    private void FeatureDuplicate()
    {
        var clone = Mapper.Map<FeatureEstimate>(Feature);

        clone.Id = Guid.NewGuid().ToString("N");
        clone.Name += " - Copy";

        Epic.Features.Add(clone);

        Store.NotifyStateChanged();
    }

    private async Task FeatureMove()
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(FeatureMoveModal.Project), Store.Model);
        parameters.Add(nameof(FeatureMoveModal.Feature), Feature);

        var messageForm = Modal.Show<FeatureMoveModal>("Move Feature", parameters);
        var result = await messageForm.Result;

        Store.NotifyStateChanged();
    }
}
