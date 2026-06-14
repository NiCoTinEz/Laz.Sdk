using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Media;

/// <summary>Response for <c>/media/video/quota/get</c>.</summary>
public sealed record GetVideoQuotaResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public GetVideoQuotaData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="GetVideoQuotaResponse"/>.</summary>
public sealed record GetVideoQuotaData
{
    [JsonPropertyName("total_quota")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? TotalQuota { get; init; }

    [JsonPropertyName("used_quota")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? UsedQuota { get; init; }

    [JsonPropertyName("remaining_quota")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? RemainingQuota { get; init; }
}
