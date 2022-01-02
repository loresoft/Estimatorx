using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Organizations;

[Authorize]
public partial class Invites
{
    [Parameter]
    public string Id { get; set; }
}
