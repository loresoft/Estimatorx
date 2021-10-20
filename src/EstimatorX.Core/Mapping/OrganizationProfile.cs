using AutoMapper;
using EstimatorX.Core.Entities;
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Mapping
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<OrganizationModel, Organization>();

            CreateMap<Organization, OrganizationModel>();
        }
    }
}
