using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/brands/get</c>.</summary>
public sealed record GetBrandsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazBrand>? Data { get; init; }
}

/// <summary>Brand entity.</summary>
public sealed record LazBrand
{
    [JsonPropertyName("brand_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long BrandId { get; init; }

    [JsonPropertyName("name_en")]        public string? NameEn { get; init; }
    [JsonPropertyName("name_th")]        public string? NameTh { get; init; }
    [JsonPropertyName("name_id")]        public string? NameId { get; init; }
    [JsonPropertyName("name_ph")]        public string? NamePh { get; init; }
    [JsonPropertyName("name_vn")]        public string? NameVn { get; init; }
    [JsonPropertyName("name_my")]        public string? NameMy { get; init; }
    [JsonPropertyName("global_brand_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? GlobalBrandId { get; init; }
}
