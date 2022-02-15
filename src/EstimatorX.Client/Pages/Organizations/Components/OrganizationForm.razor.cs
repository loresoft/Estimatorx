using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Organizations.Components;

public partial class OrganizationForm
{
    [Inject]
    public OrganizationStore OrganizationStore { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }

    public Organization Organization => OrganizationStore?.Model;

    protected bool IsOwner() => Organization?.Members?.Any(m => m.Id == UserStore?.Model?.Id && m.IsOwner) == true;
}
