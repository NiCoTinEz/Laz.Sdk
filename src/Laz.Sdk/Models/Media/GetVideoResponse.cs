using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Media;

/// <summary>Response for <c>/media/video/get</c>.</summary>
public sealed record GetVideoResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public GetVideoData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="GetVideoResponse"/>.</summary>
public sealed record GetVideoData
{
    [JsonPropertyName("video_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? VideoId { get; init; }

    [JsonPropertyName("cover_url")]    public string? CoverUrl { get; init; }
    [JsonPropertyName("video_url")]    public string? VideoUrl { get; init; }
    [JsonPropertyName("state")]        public string? State { get; init; }
    [JsonPropertyName("title")]        public string? Title { get; init; }
    [JsonPropertyName("duration")]     public string? Duration { get; init; }
    [JsonPropertyName("file_size")]    public string? FileSize { get; init; }
    [JsonPropertyName("created_time")] public string? CreatedTime { get; init; }
}
