using AutoMapper;

using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Mapping;

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

        CreateMap<Project, Template>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Created, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.Updated, opt => opt.Ignore())
            .ForMember(d => d.UpdatedBy, opt => opt.Ignore());

        CreateMap<Template, Project>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Name, opt => opt.Ignore())
            .ForMember(d => d.OrganizationId, opt => opt.Ignore())
            .ForMember(d => d.Description, opt => opt.Ignore())
            .ForMember(d => d.Created, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.Updated, opt => opt.Ignore())
            .ForMember(d => d.UpdatedBy, opt => opt.Ignore());
    }
}
