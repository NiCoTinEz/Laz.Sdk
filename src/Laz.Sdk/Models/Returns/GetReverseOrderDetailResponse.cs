using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Returns;

/// <summary>Response envelope for <c>/order/reverse/return/detail/list</c>.</summary>
public sealed record GetReverseOrderDetailResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public ReverseOrderDetailData? Data { get; init; }
}

/// <summary>Detailed reverse order information.</summary>
public sealed record ReverseOrderDetailData
{
    [JsonPropertyName("reverse_order_id")]         public string? ReverseOrderId { get; init; }
    [JsonPropertyName("order_id")]                 public string? OrderId { get; init; }
    [JsonPropertyName("order_number")]             public string? OrderNumber { get; init; }
    [JsonPropertyName("seller_id")]                public string? SellerId { get; init; }
    [JsonPropertyName("buyer_id")]                 public string? BuyerId { get; init; }
    [JsonPropertyName("reverse_status")]           public string? ReverseStatus { get; init; }
    [JsonPropertyName("reverse_type")]             public string? ReverseType { get; init; }
    [JsonPropertyName("request_reason")]           public string? RequestReason { get; init; }
    [JsonPropertyName("reason_detail")]            public string? ReasonDetail { get; init; }
    [JsonPropertyName("created_at")]               public string? CreatedAt { get; init; }
    [JsonPropertyName("updated_at")]               public string? UpdatedAt { get; init; }
    [JsonPropertyName("reverse_order_line_list")]  public IReadOnlyList<ReverseOrderLine>? ReverseOrderLineList { get; init; }
}

/// <summary>A single line item within a reverse order.</summary>
public sealed record ReverseOrderLine
{
    [JsonPropertyName("reverse_order_line_id")] public string? ReverseOrderLineId { get; init; }
    [JsonPropertyName("order_item_id")]         public string? OrderItemId { get; init; }
    [JsonPropertyName("sku")]                   public string? Sku { get; init; }
    [JsonPropertyName("sku_name")]              public string? SkuName { get; init; }
    [JsonPropertyName("shop_sku")]              public string? ShopSku { get; init; }
    [JsonPropertyName("item_price")]            public string? ItemPrice { get; init; }
    [JsonPropertyName("quantity")]              public string? Quantity { get; init; }
    [JsonPropertyName("return_quantity")]       public string? ReturnQuantity { get; init; }
    [JsonPropertyName("reverse_status")]        public string? ReverseStatus { get; init; }
    [JsonPropertyName("reason")]                public string? Reason { get; init; }
    [JsonPropertyName("reason_detail")]         public string? ReasonDetail { get; init; }
    [JsonPropertyName("buyer_expected_action")] public string? BuyerExpectedAction { get; init; }
    [JsonPropertyName("seller_action")]         public string? SellerAction { get; init; }
    [JsonPropertyName("seller_action_detail")]  public string? SellerActionDetail { get; init; }
    [JsonPropertyName("return_shipment_provider")] public string? ReturnShipmentProvider { get; init; }
    [JsonPropertyName("return_tracking_number")]   public string? ReturnTrackingNumber { get; init; }
    [JsonPropertyName("return_awb_image")]         public string? ReturnAwbImage { get; init; }
    [JsonPropertyName("created_at")]               public string? CreatedAt { get; init; }
    [JsonPropertyName("updated_at")]               public string? UpdatedAt { get; init; }
}
