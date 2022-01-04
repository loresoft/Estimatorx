using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Services;

public interface IProjectBuilder
{
    Project UpdateProject(Project project);

    ProjectSettings UpdateSettings(ProjectSettings settings);
}
