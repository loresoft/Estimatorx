using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Services;

public interface IProjectBuilder
{
    Project Build(Project project);
    ProjectSettings UpdateSettings(ProjectSettings settings);
}