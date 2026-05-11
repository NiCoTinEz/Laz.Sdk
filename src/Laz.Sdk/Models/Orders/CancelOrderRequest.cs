namespace Laz.Sdk.Models.Orders;

/// <summary>Request for <c>/order/cancel</c> — cancels a single order item in pending status.</summary>
public sealed record CancelOrderRequest
{
    /// <summary>Order item id to cancel.</summary>
    public required long OrderItemId { get; init; }

    /// <summary>Cancel reason id (must be a valid Lazada reason code).</summary>
    public required long ReasonId { get; init; }

    /// <summary>Optional free-text reason detail.</summary>
    public string? ReasonDetail { get; init; }
}
