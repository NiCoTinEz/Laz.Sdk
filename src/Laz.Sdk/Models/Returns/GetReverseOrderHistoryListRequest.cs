namespace Laz.Sdk.Models.Returns;

/// <summary>Request for <c>/order/reverse/return/history/list</c>.</summary>
public sealed record GetReverseOrderHistoryListRequest
{
    /// <summary>Reverse order line id to retrieve history for.</summary>
    public required long ReverseOrderLineId { get; init; }

    /// <summary>Page size for pagination (optional).</summary>
    public int? PageSize { get; init; }

    /// <summary>Page number for pagination (optional).</summary>
    public int? PageNumber { get; init; }
}
