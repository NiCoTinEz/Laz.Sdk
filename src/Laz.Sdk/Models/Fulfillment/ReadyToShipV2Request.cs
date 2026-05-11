namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Request for <c>/order/package/rts</c> (Ready-To-Ship v2).</summary>
public sealed record ReadyToShipV2Request
{
    /// <summary>Packages to mark ready-to-ship.</summary>
    public required IReadOnlyList<AwbPackage> Packages { get; init; }
}
