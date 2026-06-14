using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/get</c>.</summary>
public sealed record GetSellerResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public SellerInfoData? Data { get; init; }
}

/// <summary>Seller information.</summary>
public sealed record SellerInfoData
{
    [JsonPropertyName("name_company")]         public string? NameCompany { get; init; }
    [JsonPropertyName("seller_id")]            public string? SellerId { get; init; }
    [JsonPropertyName("name")]                 public string? Name { get; init; }
    [JsonPropertyName("short_code")]           public string? ShortCode { get; init; }
    [JsonPropertyName("logo_url")]             public string? LogoUrl { get; init; }
    [JsonPropertyName("email")]                public string? Email { get; init; }
    [JsonPropertyName("cb")]                   public string? Cb { get; init; }
    [JsonPropertyName("location")]             public string? Location { get; init; }
    [JsonPropertyName("status")]               public string? Status { get; init; }

    [JsonPropertyName("verified")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool? Verified { get; init; }

    [JsonPropertyName("marketplaceEaseMode")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool? MarketplaceEaseMode { get; init; }
}
