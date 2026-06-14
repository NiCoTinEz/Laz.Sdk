using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/country/info/get</c>.</summary>
public sealed record GetCountryInfoResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<CountryInfo>? Data { get; init; }
}

/// <summary>Country information.</summary>
public sealed record CountryInfo
{
    [JsonPropertyName("country_code")] public string? CountryCode { get; init; }
    [JsonPropertyName("country_name")] public string? CountryName { get; init; }
    [JsonPropertyName("currency")]     public string? Currency { get; init; }
    [JsonPropertyName("timezone")]     public string? Timezone { get; init; }
    [JsonPropertyName("language")]     public string? Language { get; init; }
    [JsonPropertyName("status")]       public string? Status { get; init; }
}
