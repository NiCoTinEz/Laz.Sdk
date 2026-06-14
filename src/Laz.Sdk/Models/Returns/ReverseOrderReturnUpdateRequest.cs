namespace Laz.Sdk.Models.Returns;

/// <summary>Request for <c>/order/reverse/return/update</c>.</summary>
public sealed record ReverseOrderReturnUpdateRequest
{
    /// <summary>Reverse order line id to update.</summary>
    public required long ReverseOrderLineId { get; init; }

    /// <summary>Action to perform (e.g. "accept", "refund", "reject").</summary>
    public required string Action { get; init; }

    /// <summary>Optional reason for the action.</summary>
    public string? Reason { get; init; }

    /// <summary>Optional reason detail for the action.</summary>
    public string? ReasonDetail { get; init; }
}
