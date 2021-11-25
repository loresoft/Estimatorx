
using EstimatorX.Core.Commands;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using MediatR;
using MediatR.CommandQuery.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers;

public class UserController : EntityCommandControllerBase<string, UserModel, UserModel, UserModel, UserModel>
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("me")]
    [AllowAnonymous]
    public async Task<ActionResult<UserModel>> Me(CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated != true)
            return Ok(new UserModel());

        var userId = User.GetUserId();
        if (userId == null)
            return Ok(new UserModel());

        var user = new UserModel();
        user.Id = userId;
        user.Name = User.GetName();
        user.Email = User.GetEmail();
        user.Provider = User.GetProvider();

        var command = new UserUpdateCommand(User, userId, user);
        var model = await Mediator.Send(command, cancellationToken);

        return Ok(model);
    }

    public override Task<ActionResult<UserModel>> Create(CancellationToken cancellationToken, UserModel createModel)
    {
        return base.Create(cancellationToken, createModel);
    }
}
