using EstimatorX.Shared.Models;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Client.Stores
{
    public class ProjectStore : StoreBase<ProjectModel>
    {
        public ProjectStore(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }

}
