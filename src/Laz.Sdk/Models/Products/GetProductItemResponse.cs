using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/item/get</c>.</summary>
public sealed record GetProductItemResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public GetProductItemData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <c>/product/item/get</c>.</summary>
public sealed record GetProductItemData
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("primary_category")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? PrimaryCategory { get; init; }

    [JsonPropertyName("attributes")]        public string? Attributes { get; init; }
    [JsonPropertyName("skus")]              public string? Skus { get; init; }
    [JsonPropertyName("images")]            public IReadOnlyList<string>? Images { get; init; }
    [JsonPropertyName("item_name")]         public string? ItemName { get; init; }
    [JsonPropertyName("item_name_mobile")]  public string? ItemNameMobile { get; init; }
    [JsonPropertyName("short_description")] public string? ShortDescription { get; init; }
    [JsonPropertyName("description")]       public string? Description { get; init; }
    [JsonPropertyName("status")]            public string? Status { get; init; }
    [JsonPropertyName("created_time")]      public string? CreatedTime { get; init; }
    [JsonPropertyName("updated_time")]      public string? UpdatedTime { get; init; }
    [JsonPropertyName("spu_id")]            public string? SpuId { get; init; }
    [JsonPropertyName("tax_class")]         public string? TaxClass { get; init; }
    [JsonPropertyName("brand")]             public string? Brand { get; init; }
    [JsonPropertyName("brand_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? BrandId { get; init; }
}
