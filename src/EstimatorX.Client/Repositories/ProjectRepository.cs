using System.Net.Http;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories
{
    public class ProjectRepository : RepositoryBase<ProjectModel>
    {
        public ProjectRepository(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string GetBasePath()
        {
            return "/api/Project";
        }
    }
}