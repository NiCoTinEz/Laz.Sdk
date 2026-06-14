using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/performance/get</c>.</summary>
public sealed record GetSellerPerformanceResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<SellerPerformanceIndicator>? Data { get; init; }
}

/// <summary>A single performance indicator.</summary>
public sealed record SellerPerformanceIndicator
{
    [JsonPropertyName("name")]       public string? Name { get; init; }
    [JsonPropertyName("value")]      public string? Value { get; init; }
    [JsonPropertyName("threshold")]  public string? Threshold { get; init; }
    [JsonPropertyName("status")]     public string? Status { get; init; }
    [JsonPropertyName("metric")]     public string? Metric { get; init; }
    [JsonPropertyName("period")]     public string? Period { get; init; }
}
