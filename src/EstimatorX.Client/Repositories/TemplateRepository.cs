using System.Net.Http;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories
{
    public class TemplateRepository : RepositoryBase<TemplateModel>
    {
        public TemplateRepository(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string GetBasePath()
        {
            return "/api/Template";
        }
    }
}