using System.Text.Json.Serialization;

namespace Laz.Sdk.Models;

/// <summary>
/// Per-country seller information returned by the token endpoint.
/// </summary>
public sealed record LazCountryUserInfo
{
    [JsonPropertyName("country")]    public string? Country { get; init; }
    [JsonPropertyName("user_id")]    public string? UserId { get; init; }
    [JsonPropertyName("seller_id")]  public string? SellerId { get; init; }
    [JsonPropertyName("short_code")] public string? ShortCode { get; init; }
}
