namespace Laz.Sdk.Models.Orders;

/// <summary>Request for <c>/order/rts</c> (Ready-to-Ship).</summary>
public sealed record ReadyToShipRequest
{
    /// <summary>Delivery type. Lazada documents only <c>"dropship"</c> as supported.</summary>
    public string DeliveryType { get; init; } = "dropship";

    /// <summary>Order item ids to mark ready-to-ship. Wired as <c>[id1,id2,...]</c>. Must be from the same order.</summary>
    public required IReadOnlyCollection<long> OrderItemIds { get; init; }

    /// <summary>
    /// Valid shipment provider from <c>/order/shipment_providers/get</c>.
    /// (Note: Lazada names this <c>shipment_provider</c> here vs <c>shipping_provider</c> on <c>/order/pack</c>.)
    /// </summary>
    public required string ShipmentProvider { get; init; }

    /// <summary>Package tracking number assigned by the shipping provider.</summary>
    public required string TrackingNumber { get; init; }
}
