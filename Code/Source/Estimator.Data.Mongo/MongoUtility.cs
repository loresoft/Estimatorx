using System;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Estimatorx.Data.Mongo
{
    public static class MongoUtility
    {
        public static MongoUrl GetMongoUrl(string connectionName)
        {
            if (connectionName == null)
                throw new ArgumentNullException("connectionName");

            var settings = ConfigurationManager.ConnectionStrings[connectionName];
            if (settings == null)
                throw new ConfigurationErrorsException(
                    string.Format("No connection string named '{0}' could be found in the application configuration file.", connectionName));

            string connectionString = settings.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                throw new ConfigurationErrorsException(
                    string.Format("The connection string '{0}' in the application's configuration file does not contain the required connectionString attribute.", connectionName));

            var mongoUrl = new MongoUrl(settings.ConnectionString);
            return mongoUrl;
        }

        public static string NewId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}