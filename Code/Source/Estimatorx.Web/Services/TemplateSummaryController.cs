using System;
using System.Web.Http;
using Estimatorx.Data.Mongo;
using Estimatorx.Core;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Query;
using Estimatorx.Data.Mongo;

namespace Estimatorx.Web.Services
{
    [Authorize]
    public class TemplateSummaryController : ApiController
    {
        private readonly ITemplateRepository _repository;

        public TemplateSummaryController()
            : this(new TemplateRepository())
        {
        }

        public TemplateSummaryController(ITemplateRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _repository = repository;
        }

        public IHttpActionResult Get(int? page = null, int? pageSize = null, string sort = null, bool? descending = null)
        {
            var result = _repository.All()
                .ToDataResult<Template, TemplateSummary>(config => config
                    .Page(page ?? 1)
                    .PageSize(pageSize ?? 50)
                    .Sort(sort)
                    .Descending(descending ?? false)
                    .Selector(TemplateRepository.SelectSummary())
                );

            return Ok(result);
        }
    }
}