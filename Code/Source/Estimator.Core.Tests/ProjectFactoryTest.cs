using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Estimator.Core.Tests
{
    public class ProjectFactoryTest
    {
        [Fact]
        public void CreateProject()
        {
            var project = ProjectFactory.Create();
            project.Should().NotBeNull();
            project.Sections.Should().NotBeNullOrEmpty();
            project.Factors.Should().NotBeNullOrEmpty();

            var json = JsonConvert.SerializeObject(project, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
