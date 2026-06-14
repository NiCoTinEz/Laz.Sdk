using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Request for <c>/product/check</c>.</summary>
public sealed record ProductCheckRequest
{
    /// <summary>Product XML data to validate.</summary>
    [JsonPropertyName("product")]
    public string? Product { get; init; }
}

/// <summary>Response for <c>/product/check</c>.</summary>
public sealed record ProductCheckResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public ProductCheckData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for product check.</summary>
public sealed record ProductCheckData
{
    [JsonPropertyName("check_result")] public string? CheckResult { get; init; }
    [JsonPropertyName("errors")]       public IReadOnlyList<string>? Errors { get; init; }
    [JsonPropertyName("warnings")]     public IReadOnlyList<string>? Warnings { get; init; }
}
