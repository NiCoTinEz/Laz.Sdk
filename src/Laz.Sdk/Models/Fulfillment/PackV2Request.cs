namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Request for <c>/order/fulfill/pack</c> (Pack v2).</summary>
public sealed record PackV2Request
{
    /// <summary>Orders to pack. Each carries its own ids list per package.</summary>
    public required IReadOnlyList<PackOrderInput> PackOrderList { get; init; }

    /// <summary>Delivery type. Lazada documents <c>"dropship"</c> as the typical value.</summary>
    public string DeliveryType { get; init; } = "dropship";

    /// <summary>Shipment provider code from <see cref="ShipmentProviderInfo.ProviderCode"/>.</summary>
    public required string ShipmentProviderCode { get; init; }

    /// <summary>
    /// Shipping allocate type. Common values: <c>"TFS"</c> (Tencent fulfilment).
    /// Pair with <see cref="GetShipmentProvidersData.ShippingAllocateType"/>.
    /// </summary>
    public string ShippingAllocateType { get; init; } = "TFS";
}

/// <summary>One order's pack instruction.</summary>
public sealed record PackOrderInput
{
    public required string OrderId { get; init; }

    /// <summary>
    /// Order item ids grouped per package (each inner list = one package).
    /// Wired as <c>[ "[id1,id2]", "[id3]" ]</c> — array of stringified arrays.
    /// </summary>
    public required IReadOnlyList<IReadOnlyList<long>> OrderItemList { get; init; }
}
