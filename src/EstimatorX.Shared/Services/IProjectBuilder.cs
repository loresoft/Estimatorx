using EstimatorX.Domain.Definitions;
using EstimatorX.Domain.Models;

namespace EstimatorX.Services;

public interface IProjectBuilder
{
    TProject UpdateProject<TProject>(TProject? project, bool includeSample = false)
        where TProject : class, IProjectEstimate, new();

    ProjectSettings UpdateSettings(ProjectSettings settings, bool includeSample = false);

    void UpdateSamples(IProjectEstimate project);
}
