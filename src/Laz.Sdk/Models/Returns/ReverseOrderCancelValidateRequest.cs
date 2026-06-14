namespace Laz.Sdk.Models.Returns;

/// <summary>Request for <c>/order/reverse/cancel/validate</c>.</summary>
public sealed record ReverseOrderCancelValidateRequest
{
    /// <summary>Order id to validate cancellation for.</summary>
    public required long OrderId { get; init; }

    /// <summary>Optional order item id to validate cancellation for a specific item.</summary>
    public long? OrderItemId { get; init; }
}
