using System;
using Estimatorx.Data.Mongo;
using Estimatorx.Core;
using KickStart;
using NLog.Config;
using NLog.Targets;

namespace Estimatorx.Data.Mongo.Tests
{
    public static class Bootstrap
    {
        public static void Start()
        {
            Kick.Start(c => c
                .IncludeAssemblyFor<Project>()
                .IncludeAssemblyFor<ProjectRepository>()
                .UseNLog(config =>
                {
                    var consoleTarget = new ConsoleTarget();
                    consoleTarget.Layout = "${time} ${level:uppercase=true:padding=1:fixedLength=true} ${logger:shortName=true} ${message} ${exception:format=tostring}";
                    config.AddTarget("console", consoleTarget);

                    var consoleRule = new LoggingRule("*", NLog.LogLevel.Trace, consoleTarget);
                    config.LoggingRules.Add(consoleRule);
                })
                .UseAutofac()
                .UseMongoDB()
                .UseStartupTask()
            );

        }
    }
}
