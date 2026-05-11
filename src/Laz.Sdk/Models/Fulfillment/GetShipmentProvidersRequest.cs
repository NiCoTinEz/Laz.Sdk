using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Request for <c>/order/shipment/providers/get</c>.</summary>
public sealed record GetShipmentProvidersRequest
{
    /// <summary>Orders to look up shipping providers for.</summary>
    public required IReadOnlyList<ShipmentLookupOrder> Orders { get; init; }
}

/// <summary>One order entry in the shipment-provider lookup request.</summary>
public sealed record ShipmentLookupOrder
{
    [JsonPropertyName("order_id")]
    public required string OrderId { get; init; }

    /// <summary>
    /// Order item ids grouped by package. Each inner array maps to one package's items.
    /// Lazada wires this as <c>[ "[id1,id2]", "[id3]" ]</c> — an array of stringified arrays.
    /// </summary>
    [JsonPropertyName("order_item_ids")]
    public required IReadOnlyList<IReadOnlyList<long>> OrderItemIds { get; init; }
}
