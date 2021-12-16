using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Components.Shared
{
    public partial class AssumptionEdit
    {
        [Parameter]
        public List<string> Assumptions { get; set; } = new();

        private void HandleAdd()
        {
            Assumptions.Add(string.Empty);
        }

        private void HandleDelete(int index)
        {
            Assumptions.RemoveAt(index);
        }

        private void HandleReporder()
        {

        }
    }
}
