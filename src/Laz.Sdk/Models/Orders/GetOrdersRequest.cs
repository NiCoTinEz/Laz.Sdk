namespace Laz.Sdk.Models.Orders;

/// <summary>
/// Request for <c>/orders/get</c>. At least one of <see cref="UpdateAfter"/> / <see cref="CreatedAfter"/> is required by Lazada.
/// </summary>
public sealed record GetOrdersRequest
{
    /// <summary>Limit returned orders to those updated after or on this UTC moment. Wired as ISO 8601 with offset.</summary>
    public DateTimeOffset? UpdateAfter { get; init; }

    /// <summary>Limit returned orders to those updated before or on this UTC moment.</summary>
    public DateTimeOffset? UpdateBefore { get; init; }

    /// <summary>Limit returned orders to those created after or on this UTC moment.</summary>
    public DateTimeOffset? CreatedAfter { get; init; }

    /// <summary>Limit returned orders to those created before or on this UTC moment.</summary>
    public DateTimeOffset? CreatedBefore { get; init; }

    /// <summary>Status filter — pick from <see cref="OrderStatuses"/>.</summary>
    public string? Status { get; init; }

    /// <summary>Page size. Max 100.</summary>
    public int? Limit { get; init; }

    /// <summary>Pagination offset.</summary>
    public int? Offset { get; init; }

    /// <summary>Sort column.</summary>
    public OrderSortBy? SortBy { get; init; }

    /// <summary>Sort direction.</summary>
    public OrderSortDirection? SortDirection { get; init; }
}
