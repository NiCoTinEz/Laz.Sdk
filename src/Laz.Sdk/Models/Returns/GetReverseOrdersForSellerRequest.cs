namespace Laz.Sdk.Models.Returns;

/// <summary>Request for <c>/reverse/getreverseordersforseller</c>.</summary>
public sealed record GetReverseOrdersForSellerRequest
{
    /// <summary>Comma-separated list of request types to filter by (optional).</summary>
    public string? RequestTypeList { get; init; }

    /// <summary>Comma-separated list of OFC statuses to filter by (optional).</summary>
    public string? OfcStatusList { get; init; }

    /// <summary>Page offset for pagination (optional).</summary>
    public int? Offset { get; init; }

    /// <summary>Page size for pagination (optional).</summary>
    public int? PageSize { get; init; }

    /// <summary>Created after timestamp (milliseconds since epoch, optional).</summary>
    public long? CreatedAfter { get; init; }

    /// <summary>Created before timestamp (milliseconds since epoch, optional).</summary>
    public long? CreatedBefore { get; init; }
}
