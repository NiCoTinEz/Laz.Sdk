namespace Laz.Sdk.Models.Fulfillment;

/// <summary>
/// Request for <c>/order/package/sof/status/update</c> — push DBS tracking event back to Lazada.
/// </summary>
public sealed record PackageStatusUpdateForDbsRequest
{
    /// <summary>Carrier-assigned waybill number.</summary>
    public required string TrackingNumber { get; init; }

    /// <summary>Source identifier. Lazada expects <c>"OPENAPI"</c>.</summary>
    public string Source { get; init; } = "OPENAPI";

    /// <summary>Optional carrier code (e.g. <c>"SF"</c>).</summary>
    public string? CarrierCode { get; init; }

    /// <summary>Package id ("tag" on the wire).</summary>
    public required string PackageId { get; init; }

    /// <summary>Tracking event payload.</summary>
    public required DbsTrackInfo TrackInfo { get; init; }
}

/// <summary>Tracking event payload for DBS package status update.</summary>
public sealed record DbsTrackInfo
{
    public required DbsTrackLatestStatus LatestStatus { get; init; }
    public required DbsTrackLatestEvent  LatestEvent  { get; init; }
}

public sealed record DbsTrackLatestStatus
{
    public required string Status { get; init; }
    public required string SubStatus { get; init; }
    public required string SubStatusDesc { get; init; }
}

public sealed record DbsTrackLatestEvent
{
    public required string Stage { get; init; }

    /// <summary>Event time as unix milliseconds.</summary>
    public required long EventTime { get; init; }

    public required string Description { get; init; }
    public required string Location { get; init; }
}
