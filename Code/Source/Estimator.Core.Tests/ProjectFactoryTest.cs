using System;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Estimatorx.Core.Tests
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
