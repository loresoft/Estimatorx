using System;
using System.Text;
using System.Threading.Tasks;

namespace Estimator.Core.Providers
{
    public interface IProjectRepository : IEntityRepository<Project, Guid>
    {

    }
}
