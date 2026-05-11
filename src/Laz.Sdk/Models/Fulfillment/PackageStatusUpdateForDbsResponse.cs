using System.Text.Json.Serialization;
using Laz.Sdk.Json;
using Laz.Sdk.Models.Logistics;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Response envelope for <c>/order/package/sof/status/update</c>.</summary>
public sealed record PackageStatusUpdateForDbsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }

    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("module")]    public PackageStatusUpdateModule? Module { get; init; }
    [JsonPropertyName("errorCode")] public OrderTraceErrorCode? ErrorCode { get; init; }
}

public sealed record PackageStatusUpdateModule
{
    [JsonPropertyName("result")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Result { get; init; }
}
