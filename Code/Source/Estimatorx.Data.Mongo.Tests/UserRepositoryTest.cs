using System.Linq;
using Estimatorx.Data.Mongo.Security;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Estimatorx.Data.Mongo.Tests
{
    public class UserRepositoryTest : DependencyInjectionBase
    {
        public UserRepositoryTest(ITestOutputHelper outputHelper, DependencyInjectionFixture dependencyInjection)
            : base(outputHelper, dependencyInjection)
        {
        }

        [Fact]
        public void OrganizationMembers()
        {
            var userRepo = new UserRepository();

            var members = userRepo.OrganizationMembers("54a98b2f24a1b0caacd1fe18").ToList();
            members.Should().NotBeNull();
        }
    }
}
