using System;
using Estimatorx.Data.Mongo;
using Estimatorx.Core;
using KickStart;

namespace Estimatorx.Data.Mongo.Tests
{
    public static class Bootstrap
    {
        public static void Start()
        {
            Kick.Start(c => c
                .IncludeAssemblyFor<Project>()
                .IncludeAssemblyFor<ProjectRepository>()
                .UseMongoDB()
                .UseStartupTask()
            );

        }
    }
}
