using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Estimatorx.Core.Providers;
using MongoDB.Driver;

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
