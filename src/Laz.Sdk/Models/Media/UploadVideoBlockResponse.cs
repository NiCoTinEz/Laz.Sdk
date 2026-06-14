using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Media;

/// <summary>Response for <c>/media/video/block/upload</c>.</summary>
public sealed record UploadVideoBlockResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public UploadVideoBlockData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="UploadVideoBlockResponse"/>.</summary>
public sealed record UploadVideoBlockData
{
    [JsonPropertyName("e_tag")]           public string? ETag { get; init; }
    [JsonPropertyName("success")]         public bool? Success { get; init; }
    [JsonPropertyName("result_code")]     public string? ResultCode { get; init; }
    [JsonPropertyName("result_message")]  public string? ResultMessage { get; init; }
}
