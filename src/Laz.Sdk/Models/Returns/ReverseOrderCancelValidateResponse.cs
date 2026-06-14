using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Returns;

/// <summary>Response envelope for <c>/order/reverse/cancel/validate</c>.</summary>
public sealed record ReverseOrderCancelValidateResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public ReverseOrderCancelValidateData? Data { get; init; }
}

/// <summary>Validation result data.</summary>
public sealed record ReverseOrderCancelValidateData
{
    [JsonPropertyName("can_cancel")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool CanCancel { get; init; }

    [JsonPropertyName("reason")] public string? Reason { get; init; }
}
