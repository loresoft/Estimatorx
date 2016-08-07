using System;
using Autofac;
using Estimatorx.Core.Providers;

namespace Estimatorx.Data.Mongo.Modules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<LoggingRepository>()
                .As<ILoggingRepository>()
                .SingleInstance(); 
        }
    }
}