namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Request for <c>/order/digital/delivered</c>.</summary>
public sealed record DeliverDigitalRequest
{
    /// <summary>Orders to mark digitally-delivered.</summary>
    public required IReadOnlyList<DigitalDeliveryOrder> Orders { get; init; }
}

/// <summary>One order entry in <see cref="DeliverDigitalRequest"/>.</summary>
public sealed record DigitalDeliveryOrder
{
    public required string OrderId { get; init; }

    /// <summary>
    /// Order item ids grouped per package. Wired as <c>[ "[id1,id2]", "[id3]" ]</c>
    /// (array of stringified arrays, same shape as <see cref="ShipmentLookupOrder.OrderItemIds"/>).
    /// </summary>
    public required IReadOnlyList<IReadOnlyList<long>> OrderItemList { get; init; }
}
