using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Reviews;

/// <summary>Response for <c>/review/seller/reply/add</c>.</summary>
public sealed record SubmitSellerReplyResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public SubmitSellerReplyData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="SubmitSellerReplyResponse"/>.</summary>
public sealed record SubmitSellerReplyData
{
    [JsonPropertyName("success")]         public bool? Success { get; init; }
    [JsonPropertyName("result_code")]     public string? ResultCode { get; init; }
    [JsonPropertyName("result_message")]  public string? ResultMessage { get; init; }
}
