using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Orders;

/// <summary>Response envelope for <c>/order/rts</c>.</summary>
public sealed record ReadyToShipResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public ReadyToShipData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object.</summary>
public sealed record ReadyToShipData
{
    [JsonPropertyName("order_items")] public IReadOnlyList<ReadyToShipOrderItem>? OrderItems { get; init; }
}

/// <summary>Single ready-to-ship item info.</summary>
public sealed record ReadyToShipOrderItem
{
    [JsonPropertyName("order_item_id")]         public string? OrderItemId { get; init; }
    [JsonPropertyName("purchase_order_id")]     public string? PurchaseOrderId { get; init; }
    [JsonPropertyName("purchase_order_number")] public string? PurchaseOrderNumber { get; init; }
}
