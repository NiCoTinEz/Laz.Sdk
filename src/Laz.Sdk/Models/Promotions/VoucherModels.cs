using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Promotions;

/// <summary>Request for creating a seller voucher. Calls <c>/promotion/voucher/create</c>.</summary>
public sealed record CreateVoucherRequest
{
    /// <summary>Minimum order amount to qualify for the voucher.</summary>
    [JsonPropertyName("criteria_over_money")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? CriteriaOverMoney { get; init; }

    /// <summary>Voucher type: 1 = fixed, 2 = percentage.</summary>
    [JsonPropertyName("voucher_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherType { get; init; }

    /// <summary>Applicable items: &quot;all&quot; or &quot;selected&quot;.</summary>
    [JsonPropertyName("apply")]
    public string? Apply { get; init; }

    /// <summary>Collection start time (UTC milliseconds).</summary>
    [JsonPropertyName("collect_start")]
    public long CollectStart { get; init; }

    /// <summary>Display area.</summary>
    [JsonPropertyName("display_area")]
    public string? DisplayArea { get; init; }

    /// <summary>Period end time (UTC milliseconds).</summary>
    [JsonPropertyName("period_end_time")]
    public long PeriodEndTime { get; init; }

    /// <summary>Voucher name.</summary>
    [JsonPropertyName("voucher_name")]
    public string? VoucherName { get; init; }

    /// <summary>Voucher discount type: 1 = fixed amount, 2 = percentage.</summary>
    [JsonPropertyName("voucher_discount_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherDiscountType { get; init; }

    /// <summary>Discount value (amount off).</summary>
    [JsonPropertyName("offering_money_value_off")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? OfferingMoneyValueOff { get; init; }

    /// <summary>Period start time (UTC milliseconds).</summary>
    [JsonPropertyName("period_start_time")]
    public long PeriodStartTime { get; init; }

    /// <summary>Limit per buyer.</summary>
    [JsonPropertyName("limit")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Limit { get; init; }

    /// <summary>Total number of vouchers issued.</summary>
    [JsonPropertyName("issued")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Issued { get; init; }

    /// <summary>Maximum discount value for percentage-type vouchers.</summary>
    [JsonPropertyName("max_discount_offering_money_value")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? MaxDiscountOfferingMoneyValue { get; init; }

    /// <summary>Percentage discount off (for percentage-type vouchers).</summary>
    [JsonPropertyName("offering_percentage_discount_off")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? OfferingPercentageDiscountOff { get; init; }
}

/// <summary>Request for updating a seller voucher. Calls <c>/promotion/voucher/update</c>.</summary>
public sealed record UpdateVoucherRequest
{
    /// <summary>Voucher ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }

    /// <summary>Minimum order amount to qualify for the voucher.</summary>
    [JsonPropertyName("criteria_over_money")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? CriteriaOverMoney { get; init; }

    /// <summary>Voucher type: 1 = fixed, 2 = percentage.</summary>
    [JsonPropertyName("voucher_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherType { get; init; }

    /// <summary>Applicable items: &quot;all&quot; or &quot;selected&quot;.</summary>
    [JsonPropertyName("apply")]
    public string? Apply { get; init; }

    /// <summary>Collection start time (UTC milliseconds).</summary>
    [JsonPropertyName("collect_start")]
    public long? CollectStart { get; init; }

    /// <summary>Display area.</summary>
    [JsonPropertyName("display_area")]
    public string? DisplayArea { get; init; }

    /// <summary>Period end time (UTC milliseconds).</summary>
    [JsonPropertyName("period_end_time")]
    public long? PeriodEndTime { get; init; }

    /// <summary>Voucher name.</summary>
    [JsonPropertyName("voucher_name")]
    public string? VoucherName { get; init; }

    /// <summary>Voucher discount type: 1 = fixed amount, 2 = percentage.</summary>
    [JsonPropertyName("voucher_discount_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherDiscountType { get; init; }

    /// <summary>Discount value (amount off).</summary>
    [JsonPropertyName("offering_money_value_off")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? OfferingMoneyValueOff { get; init; }

    /// <summary>Period start time (UTC milliseconds).</summary>
    [JsonPropertyName("period_start_time")]
    public long? PeriodStartTime { get; init; }

    /// <summary>Limit per buyer.</summary>
    [JsonPropertyName("limit")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Limit { get; init; }

    /// <summary>Total number of vouchers issued.</summary>
    [JsonPropertyName("issued")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Issued { get; init; }

    /// <summary>Maximum discount value for percentage-type vouchers.</summary>
    [JsonPropertyName("max_discount_offering_money_value")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? MaxDiscountOfferingMoneyValue { get; init; }

    /// <summary>Percentage discount off (for percentage-type vouchers).</summary>
    [JsonPropertyName("offering_percentage_discount_off")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? OfferingPercentageDiscountOff { get; init; }
}

/// <summary>Request for getting a voucher. Calls <c>/promotion/voucher/get</c>.</summary>
public sealed record GetVoucherRequest
{
    /// <summary>Voucher type: 1 = fixed, 2 = percentage.</summary>
    [JsonPropertyName("voucher_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherType { get; init; }

    /// <summary>Voucher ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }
}

/// <summary>Request for listing vouchers. Calls <c>/promotion/vouchers/get</c>.</summary>
public sealed record GetVoucherListRequest
{
    /// <summary>Voucher type: 1 = fixed, 2 = percentage.</summary>
    [JsonPropertyName("voucher_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherType { get; init; }

    /// <summary>Page offset.</summary>
    [JsonPropertyName("offset")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Offset { get; init; }

    /// <summary>Page limit.</summary>
    [JsonPropertyName("limit")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Limit { get; init; }
}

/// <summary>Request for activating/deactivating a voucher. Calls <c>/promotion/voucher/activate</c> or <c>/promotion/voucher/deactivate</c>.</summary>
public sealed record VoucherActionRequest
{
    /// <summary>Voucher type: 1 = fixed, 2 = percentage.</summary>
    [JsonPropertyName("voucher_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherType { get; init; }

    /// <summary>Voucher ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }
}

/// <summary>Request for getting voucher products. Calls <c>/promotion/voucher/products/get</c>.</summary>
public sealed record GetVoucherProductsRequest
{
    /// <summary>Voucher type: 1 = fixed, 2 = percentage.</summary>
    [JsonPropertyName("voucher_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherType { get; init; }

    /// <summary>Voucher ID.</summary>
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

/// <summary>Request for adding/removing voucher SKUs. Calls <c>/promotion/voucher/product/sku/add</c> or <c>/promotion/voucher/product/sku/remove</c>.</summary>
public sealed record VoucherSkuRequest
{
    /// <summary>Voucher type: 1 = fixed, 2 = percentage.</summary>
    [JsonPropertyName("voucher_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherType { get; init; }

    /// <summary>Voucher ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }

    /// <summary>SKU IDs to add/remove.</summary>
    [JsonPropertyName("sku_ids")]
    public IReadOnlyList<long>? SkuIds { get; init; }
}

// ──────────────────────────────────────────────
// Response models
// ──────────────────────────────────────────────

/// <summary>Response for <c>/promotion/voucher/create</c>.</summary>
public sealed record CreateVoucherResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public CreateVoucherData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/voucher/create</c>.</summary>
public sealed record CreateVoucherData
{
    /// <summary>Created promotion ID.</summary>
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }
}

/// <summary>Response for <c>/promotion/voucher/update</c>.</summary>
public sealed record UpdateVoucherResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public UpdateVoucherData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/voucher/update</c>.</summary>
public sealed record UpdateVoucherData
{
    /// <summary>Updated promotion ID.</summary>
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }
}

/// <summary>Response for <c>/promotion/voucher/get</c>.</summary>
public sealed record GetVoucherResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public VoucherDetail? Data { get; init; }
}

/// <summary>Voucher detail.</summary>
public sealed record VoucherDetail
{
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }

    [JsonPropertyName("voucher_name")]          public string? VoucherName { get; init; }
    [JsonPropertyName("voucher_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherType { get; init; }

    [JsonPropertyName("voucher_discount_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int VoucherDiscountType { get; init; }

    [JsonPropertyName("criteria_over_money")]   public string? CriteriaOverMoney { get; init; }
    [JsonPropertyName("offering_money_value_off")] public string? OfferingMoneyValueOff { get; init; }
    [JsonPropertyName("offering_percentage_discount_off")] public string? OfferingPercentageDiscountOff { get; init; }
    [JsonPropertyName("max_discount_offering_money_value")] public string? MaxDiscountOfferingMoneyValue { get; init; }
    [JsonPropertyName("apply")]                 public string? Apply { get; init; }
    [JsonPropertyName("display_area")]          public string? DisplayArea { get; init; }
    [JsonPropertyName("collect_start")]         public string? CollectStart { get; init; }
    [JsonPropertyName("period_start_time")]     public string? PeriodStartTime { get; init; }
    [JsonPropertyName("period_end_time")]       public string? PeriodEndTime { get; init; }
    [JsonPropertyName("limit")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Limit { get; init; }

    [JsonPropertyName("issued")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Issued { get; init; }

    [JsonPropertyName("status")]                public string? Status { get; init; }
}

/// <summary>Response for <c>/promotion/vouchers/get</c>.</summary>
public sealed record GetVoucherListResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public VoucherListData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/vouchers/get</c>.</summary>
public sealed record VoucherListData
{
    [JsonPropertyName("voucher_list")]
    public IReadOnlyList<VoucherDetail>? VoucherList { get; init; }

    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Total { get; init; }
}

/// <summary>Response for voucher activate/deactivate.</summary>
public sealed record VoucherActionResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public VoucherActionResult? Data { get; init; }
}

/// <summary>Inner data for voucher activate/deactivate.</summary>
public sealed record VoucherActionResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }
}

/// <summary>Response for <c>/promotion/voucher/products/get</c>.</summary>
public sealed record GetVoucherProductsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public VoucherProductsData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/voucher/products/get</c>.</summary>
public sealed record VoucherProductsData
{
    [JsonPropertyName("products")]
    public IReadOnlyList<VoucherProduct>? Products { get; init; }

    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Total { get; init; }
}

/// <summary>A product associated with a voucher.</summary>
public sealed record VoucherProduct
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

/// <summary>Response for <c>/promotion/voucher/product/sku/add</c>.</summary>
public sealed record AddVoucherSkuResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public AddVoucherSkuData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/voucher/product/sku/add</c>.</summary>
public sealed record AddVoucherSkuData
{
    [JsonPropertyName("failed_sku_ids")]
    public IReadOnlyList<long>? FailedSkuIds { get; init; }
}

/// <summary>Response for <c>/promotion/voucher/product/sku/remove</c>.</summary>
public sealed record RemoveVoucherSkuResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public VoucherActionResult? Data { get; init; }
}
