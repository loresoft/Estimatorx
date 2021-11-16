
using AutoMapper;

using Cosmos.Abstracts;

using EstimatorX.Core.Entities;
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Models;

using MediatR.CommandQuery.Commands;
using MediatR.CommandQuery.Cosmos.Handlers;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Handlers;

public class UserUpdateHandler : EntityUpdateCommandHandler<IUserRepository, User, UserModel, UserModel>
{
    public UserUpdateHandler(ILoggerFactory loggerFactory, IUserRepository repository, IMapper mapper)
        : base(loggerFactory, repository, mapper)
    {
    }

    protected override async Task<UserModel> Process(EntityUpdateCommand<string, UserModel, UserModel> request, CancellationToken cancellationToken)
    {
        var needSave = false;

        // check for existing
        var user = await Repository.FindAsync(request.Id, cancellationToken: cancellationToken);
        if (user == null)
        {
            user = new User
            {
                Id = request.Id,
                PrivateKey = ObjectId.GenerateNewId()
            };
            needSave = true;
        }

        // update name and email
        if (user.Name != request.Model.Name)
        {
            user.Name = request.Model.Name;
            needSave = true;
        }
        if (user.Email != request.Model.Email)
        {
            user.Email = request.Model.Email;
            needSave = true;
        }
        if (user.Provider != request.Model.Provider)
        {
            user.Provider = request.Model.Provider;
            needSave = true;
        }

        // save updates
        if (!needSave)
            return Mapper.Map<UserModel>(user);

        var savedEntity = await Repository.SaveAsync(user, cancellationToken);

        // return read model
        return Mapper.Map<UserModel>(savedEntity);
    }
}
