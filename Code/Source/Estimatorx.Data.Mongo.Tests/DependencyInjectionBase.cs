using System;
using KickStart;
using Xunit;
using Xunit.Abstractions;

namespace Estimatorx.Data.Mongo.Tests
{
    [Collection("DependencyInjectionCollection")]
    public abstract class DependencyInjectionBase : UnitTestBase
    {
        public DependencyInjectionFixture DependencyInjection { get; }

        protected DependencyInjectionBase(ITestOutputHelper outputHelper, DependencyInjectionFixture dependencyInjection) : base(outputHelper)
        {
            DependencyInjection = dependencyInjection;
        }

        public IServiceProvider ServiceProvider => Kick.ServiceProvider;
    }
}
