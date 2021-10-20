using AutoMapper;
using EstimatorX.Core.Entities;
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Mapping
{
    public class TemplateProfile : Profile
    {
        public TemplateProfile()
        {
            CreateMap<TemplateModel, Template>();

            CreateMap<Template, TemplateModel>();
        }
    }
}
