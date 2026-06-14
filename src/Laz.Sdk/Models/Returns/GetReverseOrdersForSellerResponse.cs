using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Returns;

/// <summary>Response envelope for <c>/reverse/getreverseordersforseller</c>.</summary>
public sealed record GetReverseOrdersForSellerResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public ReverseOrdersForSellerData? Data { get; init; }
}

/// <summary>Paginated list of reverse orders for seller.</summary>
public sealed record ReverseOrdersForSellerData
{
    [JsonPropertyName("reverseOrders")] public IReadOnlyList<SellerReverseOrder>? ReverseOrders { get; init; }

    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Total { get; init; }

    [JsonPropertyName("totalPage")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int TotalPage { get; init; }

    [JsonPropertyName("pageSize")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int PageSize { get; init; }

    [JsonPropertyName("currentPage")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int CurrentPage { get; init; }
}

/// <summary>A reverse order in the seller listing.</summary>
public sealed record SellerReverseOrder
{
    [JsonPropertyName("reverse_order_id")]         public string? ReverseOrderId { get; init; }
    [JsonPropertyName("order_id")]                 public string? OrderId { get; init; }
    [JsonPropertyName("order_number")]             public string? OrderNumber { get; init; }
    [JsonPropertyName("reverse_status")]           public string? ReverseStatus { get; init; }
    [JsonPropertyName("reverse_type")]             public string? ReverseType { get; init; }
    [JsonPropertyName("request_reason")]           public string? RequestReason { get; init; }
    [JsonPropertyName("created_at")]               public string? CreatedAt { get; init; }
    [JsonPropertyName("updated_at")]               public string? UpdatedAt { get; init; }
    [JsonPropertyName("reverse_order_line_list")]  public IReadOnlyList<SellerReverseOrderLine>? ReverseOrderLineList { get; init; }
}

/// <summary>A line item within a seller reverse order.</summary>
public sealed record SellerReverseOrderLine
{
    [JsonPropertyName("reverse_order_line_id")] public string? ReverseOrderLineId { get; init; }
    [JsonPropertyName("order_item_id")]         public string? OrderItemId { get; init; }
    [JsonPropertyName("sku")]                   public string? Sku { get; init; }
    [JsonPropertyName("sku_name")]              public string? SkuName { get; init; }
    [JsonPropertyName("shop_sku")]              public string? ShopSku { get; init; }
    [JsonPropertyName("reverse_status")]        public string? ReverseStatus { get; init; }
    [JsonPropertyName("buyer_expected_action")] public string? BuyerExpectedAction { get; init; }
    [JsonPropertyName("seller_action")]         public string? SellerAction { get; init; }
}
