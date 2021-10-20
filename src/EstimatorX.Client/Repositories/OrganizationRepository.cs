using System;
using System.Linq;
using System.Net.Http;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories
{
    public class OrganizationRepository : RepositoryBase<OrganizationModel>
    {
        public OrganizationRepository(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string GetBasePath()
        {
            return "/api/Organization";
        }
    }
}
