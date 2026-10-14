using EstimatorX.Domain.Models;
using EstimatorX.Services;

namespace EstimatorX.Core.Tests.Services;

public class ProjectCalculatorTests
{
    private readonly ProjectCalculator _calculator = new();

    private static ProjectReadModel CreateProject(Action<ProjectSettings>? configure = null)
    {
        var settings = new ProjectSettings
        {
            EstimateRate = 100,
            UnitsPerWeek = 30,
            Multipliers =
            [
                new EstimateMultiplier { Clarity = ClarityScale.High, Confidence = ConfidenceScale.High, Value = 1.5 },
                new EstimateMultiplier { Clarity = ClarityScale.Low, Confidence = ConfidenceScale.Low, Value = 2.0 },
            ],
            RiskLevels =
            [
                new RiskLevel { Risk = "Low", Multiplier = 1.0 },
                new RiskLevel { Risk = "Medium", Multiplier = 1.5 },
                new RiskLevel { Risk = "High", Multiplier = 2.0 },
            ],
            EffortLevels =
            [
                new EffortLevel { Effort = 1, Level = "XS" },
                new EffortLevel { Effort = 5, Level = "S" },
                new EffortLevel { Effort = 13, Level = "L" },
            ],
            Overhead = [],
        };

        configure?.Invoke(settings);

        return new ProjectReadModel { Name = "Test", Settings = settings };
    }

    private static StoryEstimate CreateStory(int? estimate, ClarityScale? clarity = ClarityScale.High, ConfidenceScale? confidence = ConfidenceScale.High)
        => new() { Name = "Story", Estimate = estimate, Clarity = clarity, Confidence = confidence };

    [Fact]
    public void UpdateFeature_WithMatchingMultiplier_ComputesWeightedValues()
    {
        var project = CreateProject();
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 10, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High };

        _calculator.UpdateFeature(project, feature);

        feature.Multiplier.Should().Be(1.5);
        feature.WeightedEstimate.Should().Be(15);
        feature.EstimatedTotal.Should().Be(10);
        feature.WeightedTotal.Should().Be(15);
        feature.EstimatedCost.Should().Be(1000);
        feature.WeightedCost.Should().Be(1500);
        feature.EstimatedWeeks.Should().Be(0.3);
        feature.WeightedWeeks.Should().Be(0.5);
        feature.RiskLevel.Should().Be("Medium");
        feature.EffortLevel.Should().Be("L");
    }

    [Fact]
    public void UpdateFeature_WithNoMatchingMultiplier_UsesMinimumOfOne()
    {
        var project = CreateProject();
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 5, Clarity = ClarityScale.Medium, Confidence = ConfidenceScale.Medium };

        _calculator.UpdateFeature(project, feature);

        feature.Multiplier.Should().Be(1);
        feature.WeightedEstimate.Should().Be(5);
        feature.RiskLevel.Should().Be("Low");
        feature.EffortLevel.Should().Be("S");
    }

    [Fact]
    public void UpdateFeature_WithMultiplierBelowOne_UsesMinimumOfOne()
    {
        var project = CreateProject(s => s.Multipliers =
        [
            new EstimateMultiplier { Clarity = ClarityScale.High, Confidence = ConfidenceScale.High, Value = 0.5 },
        ]);
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 8, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High };

        _calculator.UpdateFeature(project, feature);

        feature.Multiplier.Should().Be(1);
        feature.WeightedEstimate.Should().Be(8);
    }

    [Fact]
    public void UpdateFeature_WeightedEstimate_RoundsAwayFromZero()
    {
        var project = CreateProject();
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 3, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High };

        _calculator.UpdateFeature(project, feature);

        // 3 * 1.5 = 4.5 => 5
        feature.WeightedEstimate.Should().Be(5);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    public void UpdateFeature_WithoutEstimate_ClearsComputedValues(int? estimate)
    {
        var project = CreateProject();
        var feature = new FeatureEstimate
        {
            Name = "Feature",
            Estimate = estimate,
            Clarity = ClarityScale.High,
            Confidence = ConfidenceScale.High,
            Multiplier = 3,
            WeightedEstimate = 30,
            RiskLevel = "High",
            EffortLevel = "L",
        };

        _calculator.UpdateFeature(project, feature);

        feature.Multiplier.Should().BeNull();
        feature.WeightedEstimate.Should().BeNull();
        feature.RiskLevel.Should().BeNull();
        feature.EffortLevel.Should().BeNull();
        feature.EstimatedTotal.Should().Be(0);
        feature.WeightedTotal.Should().Be(0);
        feature.EstimatedCost.Should().Be(0);
        feature.WeightedCost.Should().Be(0);
    }

    [Fact]
    public void UpdateFeature_AppliesOverheadMultipliers()
    {
        var project = CreateProject(s => s.Overhead =
        [
            new ProjectOverhead { Name = "Testing", Multiplier = 1.2 },
            new ProjectOverhead { Name = "Management", Multiplier = 1.5 },
        ]);
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 10, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High };

        _calculator.UpdateFeature(project, feature);

        // 10 * 1.2 * 1.5 = 18 ; 15 * 1.2 * 1.5 = 27
        feature.EstimatedTotal.Should().Be(18);
        feature.WeightedTotal.Should().Be(27);
        feature.EstimatedCost.Should().Be(1800);
        feature.WeightedCost.Should().Be(2700);
    }

    [Fact]
    public void UpdateFeature_SumsStoriesIntoFeatureTotals()
    {
        var project = CreateProject();
        var feature = new FeatureEstimate
        {
            Name = "Feature",
            Stories =
            [
                CreateStory(10),
                CreateStory(5, ClarityScale.Low, ConfidenceScale.Low),
            ],
        };

        _calculator.UpdateFeature(project, feature);

        feature.Stories[0].WeightedEstimate.Should().Be(15);
        feature.Stories[0].EstimatedTotal.Should().Be(10);
        feature.Stories[1].WeightedEstimate.Should().Be(10);
        feature.Stories[1].RiskLevel.Should().Be("High");

        feature.EstimatedTotal.Should().Be(15);
        feature.WeightedTotal.Should().Be(25);
        feature.EstimatedCost.Should().Be(1500);
        feature.WeightedCost.Should().Be(2500);
        feature.EstimatedWeeks.Should().Be(0.5);
        feature.WeightedWeeks.Should().Be(0.8);
    }

    [Fact]
    public void UpdateFeature_WithFeatureEstimateAndStories_CombinesTotals()
    {
        var project = CreateProject();
        var feature = new FeatureEstimate
        {
            Name = "Feature",
            Estimate = 4,
            Clarity = ClarityScale.High,
            Confidence = ConfidenceScale.High,
            Stories = [CreateStory(10)],
        };

        _calculator.UpdateFeature(project, feature);

        feature.EstimatedTotal.Should().Be(14);
        feature.WeightedTotal.Should().Be(21);
    }

    [Fact]
    public void UpdateFeature_WithoutEstimateRate_HasNullCosts()
    {
        var project = CreateProject(s => s.EstimateRate = null);
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 10, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High };

        _calculator.UpdateFeature(project, feature);

        feature.EstimatedCost.Should().BeNull();
        feature.WeightedCost.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-5)]
    public void UpdateFeature_WithInvalidUnitsPerWeek_HasNullWeeks(int? unitsPerWeek)
    {
        var project = CreateProject(s => s.UnitsPerWeek = unitsPerWeek);
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 10, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High };

        _calculator.UpdateFeature(project, feature);

        feature.EstimatedWeeks.Should().BeNull();
        feature.WeightedWeeks.Should().BeNull();
    }

    [Fact]
    public void UpdateFeature_WithNoRiskOrEffortLevels_HasNullLevels()
    {
        var project = CreateProject(s =>
        {
            s.RiskLevels = [];
            s.EffortLevels = [];
        });
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 10, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High };

        _calculator.UpdateFeature(project, feature);

        feature.RiskLevel.Should().BeNull();
        feature.EffortLevel.Should().BeNull();
    }

    [Fact]
    public void UpdateEpic_SumsFeatureTotals()
    {
        var project = CreateProject();
        var epic = new EpicEstimate
        {
            Name = "Epic",
            EstimatedTotal = 999,
            Features =
            [
                new FeatureEstimate { Name = "F1", Stories = [CreateStory(10)] },
                new FeatureEstimate { Name = "F2", Estimate = 20, Clarity = ClarityScale.Low, Confidence = ConfidenceScale.Low },
            ],
        };

        _calculator.UpdateEpic(project, epic);

        epic.EstimatedTotal.Should().Be(30);
        epic.WeightedTotal.Should().Be(55);
        epic.EstimatedCost.Should().Be(3000);
        epic.WeightedCost.Should().Be(5500);
        epic.EstimatedWeeks.Should().Be(1.0);
        epic.WeightedWeeks.Should().Be(1.8);
    }

    [Fact]
    public void UpdateEpic_WithNoFeatures_HasZeroTotals()
    {
        var project = CreateProject();
        var epic = new EpicEstimate { Name = "Epic", EstimatedTotal = 5, WeightedTotal = 5 };

        _calculator.UpdateEpic(project, epic);

        epic.EstimatedTotal.Should().Be(0);
        epic.WeightedTotal.Should().Be(0);
        epic.EstimatedCost.Should().Be(0);
        epic.WeightedCost.Should().Be(0);
        epic.EstimatedWeeks.Should().Be(0);
        epic.WeightedWeeks.Should().Be(0);
    }

    [Fact]
    public void UpdateProject_SumsEpicTotals()
    {
        var project = CreateProject();
        project.Epics =
        [
            new EpicEstimate
            {
                Name = "E1",
                Features = [new FeatureEstimate { Name = "F1", Stories = [CreateStory(10)] }],
            },
            new EpicEstimate
            {
                Name = "E2",
                Features = [new FeatureEstimate { Name = "F2", Stories = [CreateStory(20)] }],
            },
        ];

        _calculator.UpdateProject(project);

        project.Epics[0].EstimatedTotal.Should().Be(10);
        project.Epics[1].EstimatedTotal.Should().Be(20);

        project.EstimatedTotal.Should().Be(30);
        project.WeightedTotal.Should().Be(45);
        project.EstimatedCost.Should().Be(3000m);
        project.WeightedCost.Should().Be(4500m);
        project.EstimatedWeeks.Should().Be(1.0);
        project.WeightedWeeks.Should().Be(1.5);
    }

    [Fact]
    public void UpdateProject_ResetsPreviousTotals()
    {
        var project = CreateProject();
        project.EstimatedTotal = 100;
        project.WeightedTotal = 100;
        project.EstimatedCost = 100;
        project.WeightedCost = 100;

        _calculator.UpdateProject(project);

        project.EstimatedTotal.Should().Be(0);
        project.WeightedTotal.Should().Be(0);
        project.EstimatedCost.Should().Be(0);
        project.WeightedCost.Should().Be(0);
    }

    [Fact]
    public void UpdateProject_IsIdempotent()
    {
        var project = CreateProject();
        project.Epics =
        [
            new EpicEstimate
            {
                Name = "E1",
                Features = [new FeatureEstimate { Name = "F1", Estimate = 3, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High, Stories = [CreateStory(10)] }],
            },
        ];

        _calculator.UpdateProject(project);
        var estimated = project.EstimatedTotal;
        var weighted = project.WeightedTotal;

        _calculator.UpdateProject(project);

        project.EstimatedTotal.Should().Be(estimated);
        project.WeightedTotal.Should().Be(weighted);
    }

    [Theory]
    [InlineData(1, 4, 0.3)]
    [InlineData(9, 4, 2.3)]
    [InlineData(5, 20, 0.3)]
    [InlineData(10, 30, 0.3)]
    [InlineData(30, 30, 1.0)]
    [InlineData(45, 10, 4.5)]
    public void UpdateFeature_CalculatesWeeks_RoundedAwayFromZero(int estimate, int unitsPerWeek, double expectedWeeks)
    {
        var project = CreateProject(s => s.UnitsPerWeek = unitsPerWeek);
        var feature = new FeatureEstimate { Name = "Feature", Estimate = estimate, Clarity = ClarityScale.Medium, Confidence = ConfidenceScale.Medium };

        _calculator.UpdateFeature(project, feature);

        feature.EstimatedWeeks.Should().Be(expectedWeeks);
        feature.WeightedWeeks.Should().Be(expectedWeeks);
    }

    [Fact]
    public void UpdateFeature_CalculatesStoryWeeks()
    {
        var project = CreateProject(s => s.UnitsPerWeek = 10);
        var story = CreateStory(10);
        var feature = new FeatureEstimate { Name = "Feature", Stories = [story] };

        _calculator.UpdateFeature(project, feature);

        // estimated 10 / 10 = 1.0 ; weighted 15 / 10 = 1.5
        story.EstimatedWeeks.Should().Be(1.0);
        story.WeightedWeeks.Should().Be(1.5);
    }

    [Fact]
    public void UpdateFeature_CalculatesWeeksFromOverheadAdjustedTotals()
    {
        var project = CreateProject(s =>
        {
            s.UnitsPerWeek = 10;
            s.Overhead = [new ProjectOverhead { Name = "Testing", Multiplier = 2 }];
        });
        var feature = new FeatureEstimate { Name = "Feature", Estimate = 10, Clarity = ClarityScale.High, Confidence = ConfidenceScale.High };

        _calculator.UpdateFeature(project, feature);

        // estimated 10 * 2 = 20 => 2.0 ; weighted 15 * 2 = 30 => 3.0
        feature.EstimatedWeeks.Should().Be(2.0);
        feature.WeightedWeeks.Should().Be(3.0);
    }

    [Fact]
    public void UpdateProject_WithoutUnitsPerWeek_HasNullWeeksAtAllLevels()
    {
        var project = CreateProject(s => s.UnitsPerWeek = null);
        var story = CreateStory(10);
        var feature = new FeatureEstimate { Name = "Feature", Stories = [story] };
        var epic = new EpicEstimate { Name = "Epic", Features = [feature] };
        project.Epics = [epic];

        _calculator.UpdateProject(project);

        story.EstimatedWeeks.Should().BeNull();
        story.WeightedWeeks.Should().BeNull();
        feature.EstimatedWeeks.Should().BeNull();
        feature.WeightedWeeks.Should().BeNull();
        epic.EstimatedWeeks.Should().BeNull();
        epic.WeightedWeeks.Should().BeNull();
        project.EstimatedWeeks.Should().BeNull();
        project.WeightedWeeks.Should().BeNull();
    }

    [Fact]
    public void UpdateSettings_SortsRiskAndEffortLevels()
    {
        var project = CreateProject(s =>
        {
            s.RiskLevels =
            [
                new RiskLevel { Risk = "High", Multiplier = 2.0 },
                new RiskLevel { Risk = "Low", Multiplier = 1.0 },
                new RiskLevel { Risk = "Medium", Multiplier = 1.5 },
            ];
            s.EffortLevels =
            [
                new EffortLevel { Effort = 13, Level = "L" },
                new EffortLevel { Effort = 1, Level = "XS" },
                new EffortLevel { Effort = 5, Level = "S" },
            ];
        });

        _calculator.UpdateSettings(project);

        project.Settings.RiskLevels.Select(r => r.Risk).Should().ContainInOrder("Low", "Medium", "High");
        project.Settings.EffortLevels.Select(e => e.Level).Should().ContainInOrder("XS", "S", "L");
    }
}
