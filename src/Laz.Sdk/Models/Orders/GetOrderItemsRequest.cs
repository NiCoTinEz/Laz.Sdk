namespace Laz.Sdk.Models.Orders;

/// <summary>Request for <c>/order/items/get</c>.</summary>
public sealed record GetOrderItemsRequest
{
    /// <summary>Seller-Center-assigned order id.</summary>
    public required long OrderId { get; init; }
}
