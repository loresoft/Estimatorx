using System;
using Estimatorx.Core.Providers;

namespace Estimatorx.Core.Security
{
    public interface IUserRepository 
        : IEntityRepository<User, string>
    {

    }
}