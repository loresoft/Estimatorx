using System;
using Autofac;
using MongoDB.Abstracts;

namespace Estimatorx.Data.Mongo.Modules
{
    public class MongoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => MongoFactory.GetMongoUrl("EstimatorxMongo"))
                .AsSelf()
                .SingleInstance();
        }
    }
}