using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/policy/fetch</c>.</summary>
public sealed record SellerPolicyFetchResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public SellerPolicyData? Data { get; init; }
}

/// <summary>Seller policy information.</summary>
public sealed record SellerPolicyData
{
    [JsonPropertyName("policy_id")]   public string? PolicyId { get; init; }
    [JsonPropertyName("policy_name")] public string? PolicyName { get; init; }
    [JsonPropertyName("content")]     public string? Content { get; init; }
    [JsonPropertyName("version")]     public string? Version { get; init; }
    [JsonPropertyName("effective_date")] public string? EffectiveDate { get; init; }
}
