using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Orders;

/// <summary>Response envelope for <c>/order/pack</c>.</summary>
public sealed record PackOrderResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public PackOrderData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object — list of packed items.</summary>
public sealed record PackOrderData
{
    [JsonPropertyName("order_items")] public IReadOnlyList<PackedOrderItem>? OrderItems { get; init; }
}

/// <summary>Single packed item info returned by <c>/order/pack</c>.</summary>
public sealed record PackedOrderItem
{
    [JsonPropertyName("order_item_id")]         public string? OrderItemId { get; init; }
    [JsonPropertyName("purchase_order_id")]     public string? PurchaseOrderId { get; init; }
    [JsonPropertyName("purchase_order_number")] public string? PurchaseOrderNumber { get; init; }
    [JsonPropertyName("tracking_number")]       public string? TrackingNumber { get; init; }
    [JsonPropertyName("shipment_provider")]     public string? ShipmentProvider { get; init; }
    [JsonPropertyName("package_id")]            public string? PackageId { get; init; }
}
