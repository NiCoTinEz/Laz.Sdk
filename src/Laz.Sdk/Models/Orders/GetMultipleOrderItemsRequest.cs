namespace Laz.Sdk.Models.Orders;

/// <summary>Request for <c>/orders/items/get</c> — up to 50 order ids per call.</summary>
public sealed record GetMultipleOrderItemsRequest
{
    /// <summary>Order ids. Wired as <c>[id1,id2,...]</c>. Max 50.</summary>
    public required IReadOnlyCollection<long> OrderIds { get; init; }
}
