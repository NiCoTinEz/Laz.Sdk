using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Orders;

/// <summary>Response envelope for <c>/orders/items/get</c> — items grouped per order.</summary>
public sealed record GetMultipleOrderItemsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazOrderItemsGroup>? Data { get; init; }
}

/// <summary>One order's worth of line items.</summary>
public sealed record LazOrderItemsGroup
{
    [JsonPropertyName("order_id")]     public string? OrderId { get; init; }
    [JsonPropertyName("order_number")] public string? OrderNumber { get; init; }
    [JsonPropertyName("order_items")]  public IReadOnlyList<LazOrderItem>? OrderItems { get; init; }
}
