using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Services;

public interface IProjectCalculator
{
    void UpdateEpic(Project project, EpicEstimate epic);
    void UpdateFeature(Project project, FeatureEstimate feature);
    void UpdateProject(Project project);
}