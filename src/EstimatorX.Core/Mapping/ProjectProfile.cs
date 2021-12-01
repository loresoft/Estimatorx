using AutoMapper;
using EstimatorX.Core.Entities;
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectModel, Project>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Created, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.Updated, opt => opt.Ignore())
            .ForMember(d => d.UpdatedBy, opt => opt.Ignore());

        CreateMap<Project, ProjectModel>();

        CreateMap<Project, ProjectSummary>();

    }
}
