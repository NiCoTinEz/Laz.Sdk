namespace Laz.Sdk.Models.Logistics;

/// <summary>Request for <c>/logistics/tps/sellers/pickup_timeslot</c>.</summary>
public sealed record UpdatePickupTimeSlotRequest
{
    public required string SellerId { get; init; }
    public required string WarehouseCode { get; init; }

    /// <summary>Time slots as <c>HH:mm-HH:mm</c> strings (e.g. <c>"08:00-12:00"</c>).</summary>
    public required IReadOnlyList<string> PickupTimeslots { get; init; }
}
