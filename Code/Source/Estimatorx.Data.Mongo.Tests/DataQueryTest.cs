using System;
using Estimator.Data.Mongo;
using Estimatorx.Core;
using Estimatorx.Core.Query;
using FluentAssertions;
using Xunit;

namespace Estimatorx.Data.Mongo.Tests
{
    public class DataQueryTest
    {
        public DataQueryTest()
        {
            Bootstrap.Start();
        }

        [Fact]
        public void ProjectToSummaryFromRequest()
        {
            var dataRequest = new QueryRequest
            {
                Page = 1,
                PageSize = 5,
            };

            var repo = new ProjectRepository();
            repo.Should().NotBeNull();

            var result = repo.All()
                .ToDataResult<Project, ProjectSummary>(config => config
                    .Request(dataRequest)
                    .Selector(ProjectRepository.SelectSummary())
                );

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public void ProjectToSummaryWithConfig()
        {
            var repo = new ProjectRepository();
            repo.Should().NotBeNull();

            var result = repo.All()
                .ToDataResult<Project, ProjectSummary>(config => config
                    .Page(1)
                    .PageSize(5)
                    .Selector(ProjectRepository.SelectSummary())
                );

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public void ProjectToSummaryNoSelector()
        {
            var repo = new ProjectRepository();
            repo.Should().NotBeNull();

            QueryResult<ProjectSummary> result;

            Action act = () => result = repo.All()
                .ToDataResult<Project, ProjectSummary>(config => config
                    .Page(1)
                    .PageSize(5)
                );

            act.ShouldThrow<ArgumentNullException>();
        }
    }
}