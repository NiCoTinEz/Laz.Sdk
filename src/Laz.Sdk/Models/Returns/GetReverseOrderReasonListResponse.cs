using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Returns;

/// <summary>Response envelope for <c>/order/reverse/reason/list</c>.</summary>
public sealed record GetReverseOrderReasonListResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<ReverseOrderReason>? Data { get; init; }
}

/// <summary>A reason object for reverse order actions.</summary>
public sealed record ReverseOrderReason
{
    [JsonPropertyName("reason_id")]   public string? ReasonId { get; init; }
    [JsonPropertyName("reason_name")] public string? ReasonName { get; init; }
}
