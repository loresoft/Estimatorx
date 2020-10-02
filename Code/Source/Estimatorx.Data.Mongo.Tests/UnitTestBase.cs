using Xunit.Abstractions;

namespace Estimatorx.Data.Mongo.Tests
{
    public abstract class UnitTestBase
    {
        protected UnitTestBase(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;

        }

        public ITestOutputHelper OutputHelper { get; }
    }
}
