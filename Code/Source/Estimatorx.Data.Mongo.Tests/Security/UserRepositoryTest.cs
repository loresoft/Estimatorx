using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estimatorx.Data.Mongo.Security;
using FluentAssertions;
using Xunit;

namespace Estimatorx.Data.Mongo.Tests.Security
{
    public class UserRepositoryTest : TestBase
    {

        [Fact]
        public void OrganizationMembers()
        {
            var userRepo = new UserRepository();

            var members = userRepo.OrganizationMembers("54a98b2f24a1b0caacd1fe18").ToList();
            members.Should().NotBeNull();
        }
    }
}
