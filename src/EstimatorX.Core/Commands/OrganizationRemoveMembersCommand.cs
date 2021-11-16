using System.Security.Principal;

using MediatR.CommandQuery.Commands;
using MediatR.CommandQuery.Models;

namespace EstimatorX.Core.Commands;

public class OrganizationRemoveMembersCommand : EntityIdentifierCommand<string, CompleteModel>
{
    public OrganizationRemoveMembersCommand(IPrincipal principal, string id) : base(principal, id)
    {
    }
}