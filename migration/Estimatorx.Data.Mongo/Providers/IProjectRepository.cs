using System;

using Estimatorx.Data.Mongo;

namespace Estimatorx.Data.Mongo.Providers
{
    public interface IProjectRepository
        : IEntityRepository<Project, string>
    {

    }
}
