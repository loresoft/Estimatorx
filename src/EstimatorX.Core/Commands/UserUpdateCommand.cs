using System.Security.Principal;

using EstimatorX.Shared.Models;

using MediatR.CommandQuery.Commands;

namespace EstimatorX.Core.Commands;

public class UserUpdateCommand : EntityUpdateCommand<string, UserModel, UserModel>
{
    public UserUpdateCommand(IPrincipal principal, string id, UserModel model) : base(principal, id, model)
    {
    }
}
