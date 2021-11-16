using System.Security.Principal;

using MediatR.CommandQuery.Commands;
using MediatR.CommandQuery.Models;

namespace EstimatorX.Core.Commands;

public class OrganizationUpdateMembersCommand : EntityIdentifierCommand<string, CompleteModel>
{
    public OrganizationUpdateMembersCommand(IPrincipal principal, string id) : base(principal, id)
    {
    }
}
