using System;
using System.Linq;
using Estimatorx.Data.Mongo;
using Estimatorx.Core;
using FluentAssertions;
using Xunit;

namespace Estimatorx.Data.Mongo.Tests
{
    public class ProjectRepositoryTest
    {
        public ProjectRepositoryTest()
        {
            Bootstrap.Start();
        }

        [Fact]
        public void SaveProject()
        {
            var project = ProjectFactory.Create();
            project.Should().NotBeNull();
            project.Sections.Should().NotBeNullOrEmpty();
            project.Factors.Should().NotBeNullOrEmpty();

            var estimate = project.Sections[0].Tasks[0];
            estimate.Simple = 1;
            estimate.Medium = 1;

            estimate = project.Sections[0].Tasks[1];
            estimate.Simple = 1;

            ProjectCalculator.UpdateTotals(project);

            project.TotalTasks.Should().Be(3);
            project.TotalHours.Should().Be(16);
            project.TotalWeeks.Should().Be(0.53);


            var repo = new ProjectRepository();
            repo.Insert(project);
        }

        [Fact]
        public void LoadAll()
        {
            var repo = new ProjectRepository();
            var projects = repo.All().ToList();
            projects.Should().NotBeNull();


        }

        [Fact]
        public void LoadSummary()
        {
            var repo = new ProjectRepository();
            var projects = repo.All().Where(p => p.Name.StartsWith("New")).Select(ProjectRepository.SelectSummary()).ToList();
            projects.Should().NotBeNull();


        }
    }
}
