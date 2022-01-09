using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Templates.Components;

public partial class TemplateTabs
{
    [Inject]
    public TemplateStore Store { get; set; }

    public Template Model => Store.Model;

}
