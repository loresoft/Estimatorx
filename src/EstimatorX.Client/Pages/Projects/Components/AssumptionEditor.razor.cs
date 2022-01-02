
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components
{
    public partial class AssumptionEditor
    {
        [Parameter]
        public List<string> Assumptions { get; set; } = new();

        [Parameter]
        public EventCallback AssumptionsChanged { get; set; }


        private void HandleAdd()
        {
            Assumptions.Add(string.Empty);
            AssumptionsChanged.InvokeAsync();
        }

        private void HandleDelete(int index)
        {
            Assumptions.RemoveAt(index);
            AssumptionsChanged.InvokeAsync();
        }

        private void HandleReporder()
        {
            AssumptionsChanged.InvokeAsync();
        }
    }
}
