using EstimatorX.Core.Commands;
using EstimatorX.Shared.Models;

using MediatR;
using MediatR.CommandQuery.Mvc;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers;

public class OrganizationController
    : EntityCommandControllerBase<string, OrganizationModel, OrganizationModel, OrganizationModel, OrganizationModel>
{
    public OrganizationController(IMediator mediator) : base(mediator)
    {
    }


    protected override async Task<OrganizationModel> CreateCommand(OrganizationModel createModel, CancellationToken cancellationToken = default)
    {
        var result = await base.CreateCommand(createModel, cancellationToken);

        await UpdateMembers(result);

        return result;
    }

    protected override async Task<OrganizationModel> UpdateCommand(string id, OrganizationModel updateModel, CancellationToken cancellationToken = default)
    {
        var result = await base.UpdateCommand(id, updateModel, cancellationToken);

        await UpdateMembers(result);

        return result;
    }

    protected override async Task<OrganizationModel> UpsertCommand(string id, OrganizationModel updateModel, CancellationToken cancellationToken = default)
    {
        var result = await base.UpsertCommand(id, updateModel, cancellationToken);

        await UpdateMembers(result);

        return result;
    }

    protected override async Task<OrganizationModel> DeleteCommand(string id, CancellationToken cancellationToken = default)
    {
        var command = new OrganizationRemoveMembersCommand(User, id);
        await Mediator.Send(command);

        var result = await base.DeleteCommand(id, cancellationToken);

        return result;
    }


    private async Task UpdateMembers(OrganizationModel entity)
    {
        if (entity == null)
            return;

        var command = new OrganizationUpdateMembersCommand(User, entity.Id);
        var response = await Mediator.Send(command);
    }

}
