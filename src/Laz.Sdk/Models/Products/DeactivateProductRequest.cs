namespace Laz.Sdk.Models.Products;

/// <summary>Request for <c>/product/deactivate</c>.</summary>
public sealed record DeactivateProductRequest
{
    /// <summary>Product item ID to deactivate.</summary>
    public long ItemId { get; init; }
}
