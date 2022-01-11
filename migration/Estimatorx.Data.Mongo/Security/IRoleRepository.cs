using System;

using Estimatorx.Data.Mongo.Providers;

namespace Estimatorx.Data.Mongo.Security
{
    public interface IRoleRepository
        : IEntityRepository<Role, string>
    {

    }
}