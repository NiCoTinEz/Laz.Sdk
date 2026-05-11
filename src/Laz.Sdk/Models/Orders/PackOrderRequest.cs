namespace Laz.Sdk.Models.Orders;

/// <summary>Request for <c>/order/pack</c>.</summary>
public sealed record PackOrderRequest
{
    /// <summary>Valid shipment provider from <c>/order/shipment_providers/get</c>. Required for <c>dropship</c>.</summary>
    public required string ShippingProvider { get; init; }

    /// <summary>Delivery type. Lazada documents only <c>"dropship"</c> as supported.</summary>
    public string DeliveryType { get; init; } = "dropship";

    /// <summary>Order item ids to pack. Wired as <c>[id1,id2,...]</c>. Must be from the same order.</summary>
    public required IReadOnlyCollection<long> OrderItemIds { get; init; }
}
