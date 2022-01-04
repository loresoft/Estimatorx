using AutoMapper;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Mapping;

public class TemplateProfile : Profile
{
    public TemplateProfile()
    {
        CreateMap<Template, Template>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Created, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.Updated, opt => opt.Ignore())
            .ForMember(d => d.UpdatedBy, opt => opt.Ignore());

        CreateMap<Template, TemplateSummary>();
    }
}
