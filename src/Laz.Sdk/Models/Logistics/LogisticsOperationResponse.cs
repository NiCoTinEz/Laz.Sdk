using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Logistics;

/// <summary>
/// Shared response envelope used by the TPS / LDP / scan endpoints under
/// <c>client.Logistics.*</c>. They all return the same {success, retryable, error*}
/// shape at the top level (no nested <c>result</c> wrapper).
/// </summary>
public sealed record LogisticsOperationResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }

    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("retryable")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Retryable { get; init; }

    [JsonPropertyName("errorMessage")] public string? ErrorMessage { get; init; }
    [JsonPropertyName("errorCode")]    public string? ErrorCode { get; init; }
    [JsonPropertyName("errors")]       public IReadOnlyList<LogisticsError>? Errors { get; init; }
}

/// <summary>Single field-level error in <see cref="LogisticsOperationResponse.Errors"/>.</summary>
public sealed record LogisticsError
{
    [JsonPropertyName("field")]        public string? Field { get; init; }
    [JsonPropertyName("errorCode")]    public string? ErrorCode { get; init; }
    [JsonPropertyName("errorMessage")] public string? ErrorMessage { get; init; }
}
