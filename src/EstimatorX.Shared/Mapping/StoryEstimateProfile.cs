using AutoMapper;

using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Mapping;

public class StoryEstimateProfile : Profile
{
    public StoryEstimateProfile()
    {
        CreateMap<StoryEstimate, StoryEstimate>()
            .ForMember(d => d.Id, opt => opt.Ignore());

    }
}
