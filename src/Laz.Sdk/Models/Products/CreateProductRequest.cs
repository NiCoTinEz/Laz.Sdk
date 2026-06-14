using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Request for <c>/product/create</c>.</summary>
public sealed record CreateProductRequest
{
    /// <summary>Primary category ID.</summary>
    [JsonPropertyName("primary_category")]
    public long PrimaryCategory { get; init; }

    /// <summary>SPU ID (optional).</summary>
    [JsonPropertyName("spu_id")]
    public long? SpuId { get; init; }

    /// <summary>Global sale channels (optional).</summary>
    [JsonPropertyName("global_sale_channels")]
    public string? GlobalSaleChannels { get; init; }

    /// <summary>Product attributes as a JSON string.</summary>
    [JsonPropertyName("attributes")]
    public string? Attributes { get; init; }

    /// <summary>SKU list as a JSON string.</summary>
    [JsonPropertyName("skus")]
    public string? Skus { get; init; }

    /// <summary>Image list as a JSON string.</summary>
    [JsonPropertyName("images")]
    public string? Images { get; init; }

    /// <summary>Product XML (alternative to attributes/skus/images).</summary>
    [JsonPropertyName("product")]
    public string? Product { get; init; }
}

/// <summary>Request for <c>/product/update</c>.</summary>
public sealed record UpdateProductRequest
{
    /// <summary>Product item ID.</summary>
    [JsonPropertyName("item_id")]
    public long ItemId { get; init; }

    /// <summary>Primary category ID.</summary>
    [JsonPropertyName("primary_category")]
    public long? PrimaryCategory { get; init; }

    /// <summary>SPU ID (optional).</summary>
    [JsonPropertyName("spu_id")]
    public long? SpuId { get; init; }

    /// <summary>Global sale channels (optional).</summary>
    [JsonPropertyName("global_sale_channels")]
    public string? GlobalSaleChannels { get; init; }

    /// <summary>Product attributes as a JSON string.</summary>
    [JsonPropertyName("attributes")]
    public string? Attributes { get; init; }

    /// <summary>SKU list as a JSON string.</summary>
    [JsonPropertyName("skus")]
    public string? Skus { get; init; }

    /// <summary>Image list as a JSON string.</summary>
    [JsonPropertyName("images")]
    public string? Images { get; init; }

    /// <summary>Product XML (alternative to attributes/skus/images).</summary>
    [JsonPropertyName("product")]
    public string? Product { get; init; }
}

/// <summary>Response for <c>/product/create</c> and <c>/product/update</c>.</summary>
public sealed record CreateProductResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public CreateProductData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="CreateProductResponse"/>.</summary>
public sealed record CreateProductData
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("sku_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? SkuId { get; init; }

    [JsonPropertyName("seller_sku")] public string? SellerSku { get; init; }
}
