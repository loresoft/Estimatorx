using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estimator.Core;
using FluentAssertions;
using MongoDB.Driver.Linq;
using Xunit;

namespace Estimator.Data.Mongo.Tests
{
    public class ProjectRepositoryTest
    {
        [Fact]
        public void SaveProject()
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


            var repo = new ProjectRepository();
            repo.Insert(project);
        }
    }
}
