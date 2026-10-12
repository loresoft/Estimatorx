using System.Text.Json.Serialization;

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain;

[JsonSerializable(typeof(FeatureEstimate))]
[JsonSerializable(typeof(EpicEstimate))]
public partial class EstimatorSerializerContext : JsonSerializerContext;
