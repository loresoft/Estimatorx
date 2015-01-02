using Autofac;

namespace Estimatorx.Data.Mongo.Modules
{
    public class MongoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => MongoUtility.GetMongoUrl("EstimatorxMongo"))
                .AsSelf()
                .SingleInstance();
        }
    }
}