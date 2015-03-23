using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Estimatorx.Core.Providers;

namespace Estimatorx.Data.Mongo.Modules
{
    public class SampleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SampleGenerator>()
                .As<ISampleGenerator>()
                .SingleInstance();
        }
    }
}
