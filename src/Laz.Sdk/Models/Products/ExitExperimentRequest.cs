using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Request for <c>/product/experiment/exit</c>.</summary>
public sealed record ExitExperimentRequest
{
    /// <summary>Experiment ID to exit.</summary>
    [JsonPropertyName("experiment_id")]
    public string? ExperimentId { get; init; }
}
