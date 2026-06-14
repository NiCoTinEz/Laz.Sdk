using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.CrossBorder;

/// <summary>Response for <c>/product/global/seller/status</c>.</summary>
public sealed record GetGlobalSellerStatusResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public GlobalSellerStatusData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="GetGlobalSellerStatusResponse"/>.</summary>
public sealed record GlobalSellerStatusData
{
    [JsonPropertyName("cross_border_enabled")]
    public string? CrossBorderEnabled { get; init; }
}
