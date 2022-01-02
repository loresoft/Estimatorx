using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Organizations.Components;

public partial class OrganizationTabs
{
    [Inject]
    public OrganizationStore OrganizationStore { get; set; }

    public Organization Organization => OrganizationStore.Model;
}
