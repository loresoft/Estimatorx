using AutoMapper;

using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Mapping;

public class FeatureEstimateProfile : Profile
{
    public FeatureEstimateProfile()
    {
        CreateMap<FeatureEstimate, FeatureEstimate>()
            .ForMember(d => d.Id, opt => opt.Ignore());

    }
}
