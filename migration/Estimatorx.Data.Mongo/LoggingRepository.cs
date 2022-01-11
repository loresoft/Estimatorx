using System;
using System.Linq.Expressions;

using Estimatorx.Data.Mongo.Providers;

using MongoDB.Abstracts;
using MongoDB.Driver;

namespace Estimatorx.Data.Mongo
{
    public class LoggingRepository : MongoQuery<LogEvent, string>, ILoggingRepository
    {
        public LoggingRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        protected override string EntityKey(LogEvent entity)
        {
            return entity.Id;
        }

        protected override Expression<Func<LogEvent, bool>> KeyExpression(string key)
        {
            return log => log.Id == key;
        }

        protected override string CollectionName()
        {
            return "Logging";
        }

        protected override void EnsureIndexes(IMongoCollection<LogEvent> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<LogEvent>(
                    Builders<LogEvent>.IndexKeys
                        .Descending(s => s.Date)
                )
            );

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<LogEvent>(
                    Builders<LogEvent>.IndexKeys
                        .Ascending(s => s.Correlation)
                )
            );

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<LogEvent>(
                    Builders<LogEvent>.IndexKeys
                        .Ascending(s => s.Level)
                )
            );

        }

        public bool Exists(Expression<Func<LogEvent, bool>> criteria)
        {
            throw new NotImplementedException();
        }
    }
}
