using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Logistics;

/// <summary>
/// Request for <c>/logistics/tps/runsheets/stops</c> — 3PL pushes pickup-stop status
/// back to Lazada's TPS (Transit Pickup System).
/// </summary>
public sealed record AddOrUpdatePickupStopRequest
{
    public required string StopId { get; init; }
    public required string SellerId { get; init; }
    public required string WarehouseCode { get; init; }
    public string? DopStationId { get; init; }
    public string? DopStationName { get; init; }

    /// <summary><c>"Pickup"</c> or <c>"Drop-off"</c>.</summary>
    public required string PickupType { get; init; }

    /// <summary>One of: <c>planned</c>, <c>arrived</c>, <c>finished</c>, <c>skipped</c>, <c>removed</c>.</summary>
    public required string Status { get; init; }

    /// <summary>Actual status-reached time as unix ms.</summary>
    public required long StatusUpdateTime { get; init; }

    public string? DispatcherName { get; init; }
    public string? DispatcherContact { get; init; }
    public string? DriverId { get; init; }
    public required string DriverName { get; init; }
    public string? DriverContact { get; init; }

    /// <summary>ETA unix ms, optional.</summary>
    public long? Eta { get; init; }

    public string? SuccessVolume { get; init; }
    public string? FailedVolume { get; init; }
    public IReadOnlyList<PickupFailedVolume>? FailedVolumeList { get; init; }
}

public sealed record PickupFailedVolume
{
    [JsonPropertyName("type")]   public string? Type { get; init; }
    [JsonPropertyName("volume")] public string? Volume { get; init; }
    [JsonPropertyName("reason")] public string? Reason { get; init; }
}
