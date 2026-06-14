using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Request for <c>/product/stock/sellable/adjust</c>.</summary>
public sealed record AdjustSellableQuantityRequest
{
    /// <summary>Product item ID.</summary>
    [JsonPropertyName("item_id")]
    public long ItemId { get; init; }

    /// <summary>SKU ID (optional, use SellerSku instead if not set).</summary>
    [JsonPropertyName("sku_id")]
    public long? SkuId { get; init; }

    /// <summary>Seller SKU (optional, use SkuId instead if not set).</summary>
    [JsonPropertyName("seller_sku")]
    public string? SellerSku { get; init; }

    /// <summary>Quantity to adjust (positive to increase, negative to decrease).</summary>
    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }
}

/// <summary>Request for <c>/product/stock/sellable/update</c>.</summary>
public sealed record UpdateSellableQuantityRequest
{
    /// <summary>Product item ID.</summary>
    [JsonPropertyName("item_id")]
    public long ItemId { get; init; }

    /// <summary>SKU ID (optional, use SellerSku instead if not set).</summary>
    [JsonPropertyName("sku_id")]
    public long? SkuId { get; init; }

    /// <summary>Seller SKU (optional, use SkuId instead if not set).</summary>
    [JsonPropertyName("seller_sku")]
    public string? SellerSku { get; init; }

    /// <summary>Absolute quantity to set.</summary>
    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }
}

/// <summary>Request for <c>/price/sellable/update</c>.</summary>
public sealed record UpdatePriceRequest
{
    /// <summary>Product item ID.</summary>
    [JsonPropertyName("item_id")]
    public long ItemId { get; init; }

    /// <summary>SKU ID (optional, use SellerSku instead if not set).</summary>
    [JsonPropertyName("sku_id")]
    public long? SkuId { get; init; }

    /// <summary>Seller SKU (optional, use SkuId instead if not set).</summary>
    [JsonPropertyName("seller_sku")]
    public string? SellerSku { get; init; }

    /// <summary>New price value.</summary>
    [JsonPropertyName("price")]
    public string? Price { get; init; }
}
