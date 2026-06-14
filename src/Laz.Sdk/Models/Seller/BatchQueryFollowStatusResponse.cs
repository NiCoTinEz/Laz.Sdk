using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/shop/follow/status/batch/query</c>.</summary>
public sealed record BatchQueryFollowStatusResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<BuyerFollowStatus>? Data { get; init; }
}

/// <summary>Follow status for a single buyer.</summary>
public sealed record BuyerFollowStatus
{
    [JsonPropertyName("buyer_id")]  public string? BuyerId { get; init; }
    [JsonPropertyName("follow")]    public string? Follow { get; init; }
}
