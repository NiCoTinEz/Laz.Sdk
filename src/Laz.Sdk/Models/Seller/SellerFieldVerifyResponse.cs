using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/field/verify</c>.</summary>
public sealed record SellerFieldVerifyResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public SellerFieldVerifyResultData? Data { get; init; }
}

/// <summary>Field verification result.</summary>
public sealed record SellerFieldVerifyResultData
{
    [JsonPropertyName("valid")]      public string? Valid { get; init; }
    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }
    [JsonPropertyName("error_msg")]  public string? ErrorMsg { get; init; }
}
