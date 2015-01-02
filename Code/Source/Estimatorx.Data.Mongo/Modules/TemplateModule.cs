using Autofac;
using Estimatorx.Core.Providers;

namespace Estimatorx.Data.Mongo.Modules
{
    public class TemplateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TemplateRepository>()
                .As<ITemplateRepository>()
                .SingleInstance();
        }
    }
}