using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Media;

/// <summary>Response for <c>/image/upload</c>.</summary>
public sealed record UploadImageResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public UploadImageData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="UploadImageResponse"/>.</summary>
public sealed record UploadImageData
{
    [JsonPropertyName("image_url")]    public string? ImageUrl { get; init; }
    [JsonPropertyName("hash_code")]    public string? HashCode { get; init; }
}
