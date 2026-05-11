namespace Laz.Sdk.Models.Orders;

/// <summary>Request for <c>/order/get</c>.</summary>
public sealed record GetOrderRequest
{
    /// <summary>Seller-Center-assigned order id.</summary>
    public required long OrderId { get; init; }
}
