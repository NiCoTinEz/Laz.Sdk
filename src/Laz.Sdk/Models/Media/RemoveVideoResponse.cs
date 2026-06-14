using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Media;

/// <summary>Response for <c>/media/video/remove</c>.</summary>
public sealed record RemoveVideoResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public RemoveVideoData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="RemoveVideoResponse"/>.</summary>
public sealed record RemoveVideoData
{
    [JsonPropertyName("success")]         public bool? Success { get; init; }
    [JsonPropertyName("result_code")]     public string? ResultCode { get; init; }
    [JsonPropertyName("result_message")]  public string? ResultMessage { get; init; }
}
