using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/experiment/config/query</c>.</summary>
public sealed record QueryProductExperimentConfigResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazExperimentConfig>? Data { get; init; }
}

/// <summary>Experiment configuration.</summary>
public sealed record LazExperimentConfig
{
    [JsonPropertyName("experiment_id")] public string? ExperimentId { get; init; }
    [JsonPropertyName("experiment_name")] public string? ExperimentName { get; init; }
    [JsonPropertyName("status")]        public string? Status { get; init; }
    [JsonPropertyName("description")]   public string? Description { get; init; }
}
