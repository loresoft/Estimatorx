using EstimatorX.Domain.Definitions;
using EstimatorX.Domain.Models;

namespace EstimatorX.Services;

public interface IProjectCalculator
{
    void UpdateProject(IProjectEstimate project);

    void UpdateEpic(IProjectEstimate project, EpicEstimate epic);

    void UpdateFeature(IProjectEstimate project, FeatureEstimate feature);

    void UpdateSettings(IProjectEstimate project);
}
