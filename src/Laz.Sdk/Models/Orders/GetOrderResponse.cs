using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Orders;

/// <summary>Response envelope for <c>/order/get</c>. <see cref="Data"/> is a single <see cref="LazOrder"/>.</summary>
public sealed record GetOrderResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public LazOrder? Data { get; init; }
}
