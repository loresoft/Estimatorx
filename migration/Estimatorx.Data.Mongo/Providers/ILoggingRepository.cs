using System;

namespace Estimatorx.Data.Mongo.Providers
{
    public interface ILoggingRepository
        : IEntityQuery<LogEvent, string>
    {

    }
}