using EstimatorX.Core.Services;
using EstimatorX.Service.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers;

public class UserController : ServiceControllerBase<UserService, UserModel, UserSummary>
{
    public UserController(UserService service) : base(service)
    {
    }

    [HttpGet("me")]
    [AllowAnonymous]
    public async Task<ActionResult<UserModel>> Me(CancellationToken cancellationToken)
    {
        var browserDetails = Request.GetBrowserData<BrowserDetail>();

        var user = await Service.Login(browserDetails, User, cancellationToken);

        return Ok(user);
    }

}
