using AutoMapper;
using EstimatorX.Core.Entities;
using EstimatorX.Core.Extensions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, User>();

            CreateMap<User, UserModel>()
                .ForMember(d => d.EmailHash, opt => opt.MapFrom(s => s.Email.ToMD5Hash()));
        }
    }
}
