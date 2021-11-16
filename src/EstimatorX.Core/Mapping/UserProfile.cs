using AutoMapper;

using EstimatorX.Core.Entities;
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserModel, User>();

        CreateMap<User, UserModel>();
    }
}
