using System.Threading;
using System.Threading.Tasks;

using EstimatorX.Core.Extensions;
using EstimatorX.Shared.Models;

using MediatR;
using MediatR.CommandQuery.Mvc;
using MediatR.CommandQuery.Queries;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers
{
    [Authorize(Roles = EstimatorX.Shared.Security.Roles.Administrators)]
    public class UserController : EntityCommandControllerBase<string, UserModel, UserModel, UserModel, UserModel>
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("me")]
        [AllowAnonymous]
        public async Task<ActionResult<UserModel>> Me(CancellationToken cancellationToken)
        {
            if (!User.Identity.IsAuthenticated)
                return Ok(new UserModel());

            var userId = User.GetUserId();
            if (userId == null)
                return Ok(new UserModel());

            var command = new EntityIdentifierQuery<string, UserModel>(User, userId);
            var model = await Mediator.Send(command, cancellationToken);
            model.IsAuthenticated = true;

            return Ok(model);
        }
    }
}
