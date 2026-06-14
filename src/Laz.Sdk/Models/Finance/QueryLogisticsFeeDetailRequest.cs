namespace Laz.Sdk.Models.Finance;

/// <summary>
/// Request for <c>/lbs/slb/queryLogisticsFeeDetail</c>.
/// </summary>
public sealed record QueryLogisticsFeeDetailRequest
{
    /// <summary>Seller ID (optional).</summary>
    public string? SellerId { get; init; }

    /// <summary>Request type (optional).</summary>
    public string? RequestType { get; init; }

    /// <summary>Start time filter (optional).</summary>
    public string? StartTime { get; init; }

    /// <summary>End time filter (optional).</summary>
    public string? EndTime { get; init; }

    /// <summary>Page number (optional).</summary>
    public int? PageNum { get; init; }

    /// <summary>Page size (optional).</summary>
    public int? PageSize { get; init; }
}
