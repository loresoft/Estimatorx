using AutoMapper;

using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Mapping;

public class EpicEstimateProfile : Profile
{
    public EpicEstimateProfile()
    {
        CreateMap<EpicEstimate, EpicEstimate>()
            .ForMember(d => d.Id, opt => opt.Ignore());

    }
}
