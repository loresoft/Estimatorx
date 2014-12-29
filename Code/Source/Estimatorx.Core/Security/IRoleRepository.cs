using System;
using Estimatorx.Core.Providers;

namespace Estimatorx.Core.Security
{
    public interface IRoleRepository 
        : IEntityRepository<Role, string>
    {

    }
}