using System;
using System.Web.Http;
using Estimatorx.Data.Mongo;
using Estimatorx.Core;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Query;
using Estimatorx.Data.Mongo;

namespace Estimatorx.Web.Services
{
    public class ProjectSummaryController : ApiController
    {
        private readonly IProjectRepository _repository;

        public ProjectSummaryController()
            : this(new ProjectRepository())
        {
        }

        public ProjectSummaryController(IProjectRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _repository = repository;
        }

        public IHttpActionResult Get(int? page = null, int? pageSize = null, string sort = null, bool? descending = null)
        {
            var result = _repository.All()
                .ToDataResult<Project, ProjectSummary>(config => config
                    .Page(page ?? 1)
                    .PageSize(pageSize ?? 50)
                    .Sort(sort)
                    .Descending(descending ?? false)
                    .Selector(ProjectRepository.SelectSummary())
                );

            return Ok(result);
        }
    }
}