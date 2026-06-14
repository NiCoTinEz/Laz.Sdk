using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Promotions;

/// <summary>Request for creating a FlexiCombo promotion. Calls <c>/promotion/flexicombo/create</c>.</summary>
public sealed record CreateFlexiComboRequest
{
    /// <summary>FlexiCombo name.</summary>
    [JsonPropertyName("flexi_combo_name")]
    public string? FlexiComboName { get; init; }

    /// <summary>FlexiCombo type.</summary>
    [JsonPropertyName("flexi_combo_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int FlexiComboType { get; init; }

    /// <summary>Start time (UTC milliseconds).</summary>
    [JsonPropertyName("start_time")]
    public long StartTime { get; init; }

    /// <summary>End time (UTC milliseconds).</summary>
    [JsonPropertyName("end_time")]
    public long EndTime { get; init; }

    /// <summary>Discount type.</summary>
    [JsonPropertyName("discount_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? DiscountType { get; init; }

    /// <summary>Discount value.</summary>
    [JsonPropertyName("discount_value")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? DiscountValue { get; init; }

    /// <summary>Minimum spend amount.</summary>
    [JsonPropertyName("min_spend")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? MinSpend { get; init; }

    /// <summary>Maximum discount cap.</summary>
    [JsonPropertyName("max_discount")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? MaxDiscount { get; init; }

    /// <summary>Applicable items: &quot;all&quot; or &quot;selected&quot;.</summary>
    [JsonPropertyName("apply")]
    public string? Apply { get; init; }
}

/// <summary>Request for getting FlexiCombo details. Calls <c>/promotion/flexicombo/details</c>.</summary>
public sealed record GetFlexiComboDetailsRequest
{
    /// <summary>FlexiCombo ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }
}

/// <summary>Request for updating a FlexiCombo promotion. Calls <c>/promotion/flexicombo/update</c>.</summary>
public sealed record UpdateFlexiComboRequest
{
    /// <summary>FlexiCombo ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }

    /// <summary>FlexiCombo name.</summary>
    [JsonPropertyName("flexi_combo_name")]
    public string? FlexiComboName { get; init; }

    /// <summary>FlexiCombo type.</summary>
    [JsonPropertyName("flexi_combo_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int FlexiComboType { get; init; }

    /// <summary>Start time (UTC milliseconds).</summary>
    [JsonPropertyName("start_time")]
    public long? StartTime { get; init; }

    /// <summary>End time (UTC milliseconds).</summary>
    [JsonPropertyName("end_time")]
    public long? EndTime { get; init; }

    /// <summary>Discount type.</summary>
    [JsonPropertyName("discount_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? DiscountType { get; init; }

    /// <summary>Discount value.</summary>
    [JsonPropertyName("discount_value")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? DiscountValue { get; init; }

    /// <summary>Minimum spend amount.</summary>
    [JsonPropertyName("min_spend")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? MinSpend { get; init; }

    /// <summary>Maximum discount cap.</summary>
    [JsonPropertyName("max_discount")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal? MaxDiscount { get; init; }

    /// <summary>Applicable items: &quot;all&quot; or &quot;selected&quot;.</summary>
    [JsonPropertyName("apply")]
    public string? Apply { get; init; }
}

/// <summary>Request for listing FlexiCombo promotions. Calls <c>/promotion/flexicombo/list</c>.</summary>
public sealed record GetFlexiComboListRequest
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

/// <summary>Request for activating/deactivating a FlexiCombo promotion.</summary>
public sealed record FlexiComboActionRequest
{
    /// <summary>FlexiCombo ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }
}

/// <summary>Request for adding/removing FlexiCombo products.</summary>
public sealed record FlexiComboProductsRequest
{
    /// <summary>FlexiCombo ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }

    /// <summary>Product IDs to add/remove.</summary>
    [JsonPropertyName("product_ids")]
    public IReadOnlyList<long>? ProductIds { get; init; }
}

// ──────────────────────────────────────────────
// Response models
// ──────────────────────────────────────────────

/// <summary>Response for <c>/promotion/flexicombo/create</c>.</summary>
public sealed record CreateFlexiComboResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public CreateFlexiComboData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/flexicombo/create</c>.</summary>
public sealed record CreateFlexiComboData
{
    /// <summary>Created promotion ID.</summary>
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }
}

/// <summary>Response for <c>/promotion/flexicombo/details</c>.</summary>
public sealed record GetFlexiComboDetailsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FlexiComboDetail? Data { get; init; }
}

/// <summary>FlexiCombo promotion detail.</summary>
public sealed record FlexiComboDetail
{
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }

    [JsonPropertyName("flexi_combo_name")] public string? FlexiComboName { get; init; }
    [JsonPropertyName("flexi_combo_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int FlexiComboType { get; init; }

    [JsonPropertyName("start_time")]       public string? StartTime { get; init; }
    [JsonPropertyName("end_time")]         public string? EndTime { get; init; }
    [JsonPropertyName("discount_type")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int DiscountType { get; init; }

    [JsonPropertyName("discount_value")]   public string? DiscountValue { get; init; }
    [JsonPropertyName("min_spend")]        public string? MinSpend { get; init; }
    [JsonPropertyName("max_discount")]     public string? MaxDiscount { get; init; }
    [JsonPropertyName("apply")]            public string? Apply { get; init; }
    [JsonPropertyName("status")]           public string? Status { get; init; }
}

/// <summary>Response for <c>/promotion/flexicombo/update</c>.</summary>
public sealed record UpdateFlexiComboResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public UpdateFlexiComboData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/flexicombo/update</c>.</summary>
public sealed record UpdateFlexiComboData
{
    [JsonPropertyName("promotion_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PromotionId { get; init; }
}

/// <summary>Response for <c>/promotion/flexicombo/list</c>.</summary>
public sealed record GetFlexiComboListResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FlexiComboListData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/flexicombo/list</c>.</summary>
public sealed record FlexiComboListData
{
    [JsonPropertyName("flexi_combo_list")]
    public IReadOnlyList<FlexiComboDetail>? FlexiComboList { get; init; }

    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Total { get; init; }
}

/// <summary>Response for FlexiCombo activate/deactivate.</summary>
public sealed record FlexiComboActionResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FlexiComboActionResult? Data { get; init; }
}

/// <summary>Inner data for FlexiCombo activate/deactivate.</summary>
public sealed record FlexiComboActionResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }
}

/// <summary>Response for <c>/promotion/flexicombo/products/add</c>.</summary>
public sealed record AddFlexiComboProductsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public AddFlexiComboProductsData? Data { get; init; }
}

/// <summary>Inner data for <c>/promotion/flexicombo/products/add</c>.</summary>
public sealed record AddFlexiComboProductsData
{
    [JsonPropertyName("failed_product_ids")]
    public IReadOnlyList<long>? FailedProductIds { get; init; }
}

/// <summary>Response for <c>/promotion/flexicombo/products/remove</c>.</summary>
public sealed record RemoveFlexiComboProductsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public FlexiComboActionResult? Data { get; init; }
}
