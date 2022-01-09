using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Services;

public interface IProjectBuilder
{
    Project UpdateProject(Project project, bool includeSample = false);

    ProjectSettings UpdateSettings(ProjectSettings settings, bool includeSample = false);

    void UpdateSamples(Project project);
}
