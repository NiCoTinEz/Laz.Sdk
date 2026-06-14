namespace Laz.Sdk.Models.Products;

/// <summary>Request for <c>/products/get</c>.</summary>
public sealed record GetProductsRequest
{
    /// <summary>Filter by seller SKU.</summary>
    public string? SellerSku { get; init; }

    /// <summary>Filter by shop SKU.</summary>
    public string? ShopSku { get; init; }

    /// <summary>Filter by SKU seller list (comma-separated).</summary>
    public string? SkuSellerList { get; init; }

    /// <summary>Page offset.</summary>
    public int? Offset { get; init; }

    /// <summary>Page limit (max 100).</summary>
    public int? Limit { get; init; }

    /// <summary>Filter by product IDs (comma-separated).</summary>
    public string? ItemIds { get; init; }

    /// <summary>Filter by product name.</summary>
    public string? ProductName { get; init; }

    /// <summary>Filter by options (e.g. "live").</summary>
    public string? Options { get; init; }

    /// <summary>Timestamp filter (UTC created after).</summary>
    public string? CreatedAfter { get; init; }

    /// <summary>Timestamp filter (UTC created before).</summary>
    public string? CreatedBefore { get; init; }

    /// <summary>Timestamp filter (UTC updated after).</summary>
    public string? UpdatedAfter { get; init; }

    /// <summary>Timestamp filter (UTC updated before).</summary>
    public string? UpdatedBefore { get; init; }

    /// <summary>Filter by status.</summary>
    public string? Status { get; init; }
}
