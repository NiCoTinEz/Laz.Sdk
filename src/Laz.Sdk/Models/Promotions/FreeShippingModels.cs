using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Promotions;

/// <summary>Request for creating a free shipping promotion. Calls <c>/promotion/freeshipping/create</c>.</summary>
public sealed record CreateFreeShippingRequest
{
    /// <summary>Free shipping name.</summary>
    [JsonPropertyName("free_shipping_name")]
    public string? FreeShippingName { get; init; }

    /// <summary>Free shipping type.</summary>
    [JsonPropertyName("free_shipping_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int FreeShippingType { get; init; }

    /// <summary>Start time (UTC milliseconds).</summary>
    [JsonPropertyName("start_time")]
    public long StartTime { get; init; }

    /// <summary>End time (UTC milliseconds).</summary>
    [JsonPropertyName("end_time")]
    public long EndTime { get; init; }

    /// <summary>Applicable items: &quot;all&quot; or &quot;selected&quot;.</summary>
    [JsonPropertyName("apply")]
    public string? Apply { get; init; }

    /// <summary>Minimum order amount to qualify.</summary>
    [JsonPropertyName("criteria_over_money")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? CriteriaOverMoney { get; init; }

    /// <summary>Display area.</summary>
    [JsonPropertyName("display_area")]
    public string? DisplayArea { get; init; }
}

/// <summary>Request for updating a free shipping promotion. Calls <c>/promotion/freeshipping/update</c>.</summary>
public sealed record UpdateFreeShippingRequest
{
    /// <summary>Free shipping ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }

    /// <summary>Free shipping name.</summary>
    [JsonPropertyName("free_shipping_name")]
    public string? FreeShippingName { get; init; }

    /// <summary>Free shipping type.</summary>
    [JsonPropertyName("free_shipping_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int FreeShippingType { get; init; }

    /// <summary>Start time (UTC milliseconds).</summary>
    [JsonPropertyName("start_time")]
    public long? StartTime { get; init; }

    /// <summary>End time (UTC milliseconds).</summary>
    [JsonPropertyName("end_time")]
    public long? EndTime { get; init; }

    /// <summary>Applicable items: &quot;all&quot; or &quot;selected&quot;.</summary>
    [JsonPropertyName("apply")]
    public string? Apply { get; init; }

    /// <summary>Minimum order amount to qualify.</summary>
    [JsonPropertyName("criteria_over_money")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? CriteriaOverMoney { get; init; }

    /// <summary>Display area.</summary>
    [JsonPropertyName("display_area")]
    public string? DisplayArea { get; init; }
}

/// <summary>Request for getting a free shipping promotion. Calls <c>/promotion/freeshipping/get</c>.</summary>
public sealed record GetFreeShippingRequest
{
    /// <summary>Free shipping ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }
}

/// <summary>Request for listing free shipping promotions. Calls <c>/promotion/freeshippings/get</c>.</summary>
public sealed record GetFreeShippingListRequest
{
    /// <summary>Page offset.</summary>
    [JsonPropertyName("offset")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Offset { get; init; }

    /// <summary>Page limit.</summary>
    [JsonPropertyName("limit")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Limit { get; init; }
}

/// <summary>Request for activating/deactivating a free shipping promotion.</summary>
public sealed record FreeShippingActionRequest
{
    /// <summary>Free shipping ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }
}

/// <summary>Request for getting free shipping products. Calls <c>/promotion/freeshipping/products/get</c>.</summary>
public sealed record GetFreeShippingProductsRequest
{
    /// <summary>Free shipping ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }

    /// <summary>Page offset.</summary>
    [JsonPropertyName("offset")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Offset { get; init; }

    /// <summary>Page limit.</summary>
    [JsonPropertyName("limit")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Limit { get; init; }
}

/// <summary>Request for adding/removing free shipping SKUs.</summary>
public sealed record FreeShippingSkuRequest
{
    /// <summary>Free shipping ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }

    /// <summary>SKU IDs to add/remove.</summary>
    [JsonPropertyName("sku_ids")]
    public IReadOnlyList<long>? SkuIds { get; init; }
}

// ──────────────────────────────────────────────
// Response models
// ──────────────────────────────────────────────

/// <summary>Response for <c>/promotion/freeshipping/create</c>.</summary>
public sealed record CreateFreeShippingResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public CreateFreeShippingData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/freeshipping/create</c>.</summary>
public sealed record CreateFreeShippingData
{
    /// <summary>Created promotion ID.</summary>
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }
}

/// <summary>Response for <c>/promotion/freeshipping/update</c>.</summary>
public sealed record UpdateFreeShippingResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public UpdateFreeShippingData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/freeshipping/update</c>.</summary>
public sealed record UpdateFreeShippingData
{
    /// <summary>Updated promotion ID.</summary>
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }
}

/// <summary>Response for <c>/promotion/freeshipping/get</c>.</summary>
public sealed record GetFreeShippingResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FreeShippingDetail? Data { get; init; }
}

/// <summary>Free shipping promotion detail.</summary>
public sealed record FreeShippingDetail
{
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }

    [JsonPropertyName("free_shipping_name")] public string? FreeShippingName { get; init; }
    [JsonPropertyName("free_shipping_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int FreeShippingType { get; init; }

    [JsonPropertyName("start_time")]         public string? StartTime { get; init; }
    [JsonPropertyName("end_time")]           public string? EndTime { get; init; }
    [JsonPropertyName("apply")]              public string? Apply { get; init; }
    [JsonPropertyName("criteria_over_money")] public string? CriteriaOverMoney { get; init; }
    [JsonPropertyName("display_area")]       public string? DisplayArea { get; init; }
    [JsonPropertyName("status")]             public string? Status { get; init; }
}

/// <summary>Response for <c>/promotion/freeshippings/get</c>.</summary>
public sealed record GetFreeShippingListResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FreeShippingListData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/freeshippings/get</c>.</summary>
public sealed record FreeShippingListData
{
    [JsonPropertyName("free_shipping_list")]
    public IReadOnlyList<FreeShippingDetail>? FreeShippingList { get; init; }

    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Total { get; init; }
}

/// <summary>Response for free shipping activate/deactivate.</summary>
public sealed record FreeShippingActionResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FreeShippingActionResult? Data { get; init; }
}

/// <summary>Inner data for free shipping activate/deactivate.</summary>
public sealed record FreeShippingActionResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }
}

/// <summary>Response for <c>/promotion/freeshipping/products/get</c>.</summary>
public sealed record GetFreeShippingProductsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FreeShippingProductsData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/freeshipping/products/get</c>.</summary>
public sealed record FreeShippingProductsData
{
    [JsonPropertyName("products")]
    public IReadOnlyList<FreeShippingProduct>? Products { get; init; }

    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Total { get; init; }
}

/// <summary>A product associated with a free shipping promotion.</summary>
public sealed record FreeShippingProduct
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long ItemId { get; init; }

    [JsonPropertyName("sku_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long SkuId { get; init; }

    [JsonPropertyName("sku_name")]   public string? SkuName { get; init; }
    [JsonPropertyName("item_name")]  public string? ItemName { get; init; }
}

/// <summary>Response for <c>/promotion/freeshipping/product/sku/add</c>.</summary>
public sealed record AddFreeShippingSkuResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public AddFreeShippingSkuData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/freeshipping/product/sku/add</c>.</summary>
public sealed record AddFreeShippingSkuData
{
    [JsonPropertyName("failed_sku_ids")]
    public IReadOnlyList<long>? FailedSkuIds { get; init; }
}

/// <summary>Response for <c>/promotion/freeshipping/product/sku/remove</c>.</summary>
public sealed record RemoveFreeShippingSkuResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FreeShippingActionResult? Data { get; init; }
}
