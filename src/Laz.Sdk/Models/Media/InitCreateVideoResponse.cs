using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Media;

/// <summary>Response for <c>/media/video/block/create</c>.</summary>
public sealed record InitCreateVideoResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public InitCreateVideoData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="InitCreateVideoResponse"/>.</summary>
public sealed record InitCreateVideoData
{
    [JsonPropertyName("upload_id")]       public string? UploadId { get; init; }
    [JsonPropertyName("success")]         public bool? Success { get; init; }
    [JsonPropertyName("result_code")]     public string? ResultCode { get; init; }
    [JsonPropertyName("result_message")]  public string? ResultMessage { get; init; }
}
