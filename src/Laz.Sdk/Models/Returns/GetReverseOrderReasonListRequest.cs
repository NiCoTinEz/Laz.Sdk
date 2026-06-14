namespace Laz.Sdk.Models.Returns;

/// <summary>Request for <c>/order/reverse/reason/list</c>.</summary>
public sealed record GetReverseOrderReasonListRequest
{
    /// <summary>Reverse order line id to retrieve reasons for.</summary>
    public required long ReverseOrderLineId { get; init; }
}
