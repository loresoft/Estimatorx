using AutoMapper;

using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Mapping;

public class InviteProfile : Profile
{
    public InviteProfile()
    {
        CreateMap<Invite, Invite>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.SecurityKey, opt => opt.Ignore())
            .ForMember(d => d.Created, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.Updated, opt => opt.Ignore())
            .ForMember(d => d.UpdatedBy, opt => opt.Ignore());

        CreateMap<Invite, InviteSummary>();
    }
}
