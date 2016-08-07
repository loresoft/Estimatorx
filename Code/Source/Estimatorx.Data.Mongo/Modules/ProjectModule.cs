using System;
using Autofac;
using Estimatorx.Core.Providers;

namespace Estimatorx.Data.Mongo.Modules
{
    public class ProjectModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ProjectRepository>()
                .As<IProjectRepository>()
                .SingleInstance(); 
        }
    }
}
