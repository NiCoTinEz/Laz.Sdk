using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Media;

/// <summary>Response for <c>/media/video/block/commit</c>.</summary>
public sealed record CompleteCreateVideoResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public CompleteCreateVideoData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="CompleteCreateVideoResponse"/>.</summary>
public sealed record CompleteCreateVideoData
{
    [JsonPropertyName("video_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? VideoId { get; init; }

    [JsonPropertyName("success")]         public bool? Success { get; init; }
    [JsonPropertyName("result_code")]     public string? ResultCode { get; init; }
    [JsonPropertyName("result_message")]  public string? ResultMessage { get; init; }
}
