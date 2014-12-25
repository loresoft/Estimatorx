using System;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Estimatorx.Core.Tests
{
    public class ProjectCalculatorTest
    {
        [Fact]
        public void UpdateTotals()
        {
            var project = ProjectFactory.Create();
            project.Should().NotBeNull();
            project.Sections.Should().NotBeNullOrEmpty();
            project.Factors.Should().NotBeNullOrEmpty();

            var estimate = project.Sections[0].Estimates[0];
            estimate.Simple = 1;
            estimate.Medium = 1;

            estimate = project.Sections[0].Estimates[1];
            estimate.Simple = 1;

            ProjectCalculator.UpdateTotals(project);

            project.TotalTasks.Should().Be(3);
            project.TotalHours.Should().Be(16);
            project.TotalWeeks.Should().Be(0.53);

            project.ContingencyHours.Should().Be(18);
            project.ContingencyWeeks.Should().Be(0.6);

            var section = project.Sections.First();
            section.TotalTasks.Should().Be(3);
            section.TotalHours.Should().Be(16);
            section.TotalWeeks.Should().Be(0.53);


            var json = JsonConvert.SerializeObject(project, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
