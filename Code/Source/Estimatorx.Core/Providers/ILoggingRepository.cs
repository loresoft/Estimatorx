using System;

namespace Estimatorx.Core.Providers
{
    public interface ILoggingRepository
        : IEntityQuery<LogEvent, string>
    {
        
    }
}