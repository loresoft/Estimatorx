using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estimator.Core;
using KickStart;

namespace Estimator.Data.Mongo.Tests
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
