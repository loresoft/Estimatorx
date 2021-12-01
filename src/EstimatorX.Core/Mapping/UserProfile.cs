using AutoMapper;

using EstimatorX.Core.Entities;
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserModel, User>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.PrivateKey, opt => opt.Ignore())
            .ForMember(d => d.Created, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.Updated, opt => opt.Ignore())
            .ForMember(d => d.UpdatedBy, opt => opt.Ignore());

        CreateMap<User, UserModel>();

        CreateMap<User, UserSummary>();
    }
}
