namespace Laz.Sdk.Models.Returns;

/// <summary>Request for <c>/order/reverse/return/detail/list</c>.</summary>
public sealed record GetReverseOrderDetailRequest
{
    /// <summary>Reverse order id to retrieve details for.</summary>
    public required long ReverseOrderId { get; init; }
}
