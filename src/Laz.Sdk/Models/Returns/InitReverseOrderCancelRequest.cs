namespace Laz.Sdk.Models.Returns;

/// <summary>Request for <c>/order/reverse/cancel/create</c>.</summary>
public sealed record InitReverseOrderCancelRequest
{
    /// <summary>Order id to initiate cancellation for.</summary>
    public required long OrderId { get; init; }

    /// <summary>Cancel reason text.</summary>
    public required string Reason { get; init; }

    /// <summary>Optional order item id to cancel a specific item.</summary>
    public long? OrderItemId { get; init; }
}
