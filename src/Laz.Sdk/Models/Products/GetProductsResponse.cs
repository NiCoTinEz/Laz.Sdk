using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/products/get</c>.</summary>
public sealed record GetProductsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public GetProductsData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <c>/products/get</c>.</summary>
public sealed record GetProductsData
{
    [JsonPropertyName("total_products")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? TotalProducts { get; init; }

    [JsonPropertyName("products")]
    public IReadOnlyList<LazProduct>? Products { get; init; }

    [JsonPropertyName("products_count")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? ProductsCount { get; init; }
}

/// <summary>Product entity surfaced by <c>/products/get</c>.</summary>
public sealed record LazProduct
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("primary_category")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? PrimaryCategory { get; init; }

    [JsonPropertyName("attributes")]              public string? Attributes { get; init; }
    [JsonPropertyName("skus")]                    public string? Skus { get; init; }
    [JsonPropertyName("images")]                  public IReadOnlyList<string>? Images { get; init; }
    [JsonPropertyName("item_name")]               public string? ItemName { get; init; }
    [JsonPropertyName("item_name_mobile")]        public string? ItemNameMobile { get; init; }
    [JsonPropertyName("short_description")]       public string? ShortDescription { get; init; }
    [JsonPropertyName("description")]             public string? Description { get; init; }
    [JsonPropertyName("status")]                  public string? Status { get; init; }
    [JsonPropertyName("created_time")]            public string? CreatedTime { get; init; }
    [JsonPropertyName("updated_time")]            public string? UpdatedTime { get; init; }
    [JsonPropertyName("spu_id")]                  public string? SpuId { get; init; }
    [JsonPropertyName("tax_class")]               public string? TaxClass { get; init; }
    [JsonPropertyName("brand")]                   public string? Brand { get; init; }
    [JsonPropertyName("brand_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? BrandId { get; init; }
}
