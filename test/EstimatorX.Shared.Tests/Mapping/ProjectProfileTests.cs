
using System;

using AutoMapper;

using EstimatorX.Shared.Mapping;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using FluentAssertions;

using Xunit;

namespace EstimatorX.Shared.Tests.Mapping;

public class ProjectProfileTests
{
    [Fact]
    public void CloneWithOutId()
    {
        var builder = new ProjectBuilder();
        var project = new Project { Id = Guid.NewGuid().ToString() };

        builder.UpdateProject(project);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProjectProfile>();
            cfg.AddProfile<EpicEstimateProfile>();
            cfg.AddProfile<FeatureEstimateProfile>();
            cfg.AddProfile<StoryEstimateProfile>();
        });

        var mapper = config.CreateMapper();

        var result = mapper.Map<Project>(project);

        result.Should().NotBeNull();
        result.Should().NotBeSameAs(project);

        result.Id.Should().NotBe(project.Id);
        result.Id.Should().BeNullOrEmpty();

        result.Name.Should().Be(project.Name);

        result.Epics.Should().HaveCount(1);

        for (int i = 0; i < result.Epics.Count; i++)
        {
            var newEpic = result.Epics[i];
            var oldEpic = project.Epics[i];

            newEpic.Should().NotBeSameAs(oldEpic);
            newEpic.Id.Should().NotBeNullOrEmpty();
            newEpic.Id.Should().NotBe(oldEpic.Id);
            newEpic.Name.Should().Be(oldEpic.Name);

            for (int x = 0; x < newEpic.Features.Count; x++)
            {
                var newFeature = newEpic.Features[x];
                var oldFeature = oldEpic.Features[x];

                newFeature.Should().NotBeSameAs(oldFeature);
                newFeature.Id.Should().NotBeNullOrEmpty();
                newFeature.Id.Should().NotBe(oldFeature.Id);
                newFeature.Name.Should().Be(oldFeature.Name);
            }
        }
    }
}
