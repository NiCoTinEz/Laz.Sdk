using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Orders;

/// <summary>Response envelope for <c>/order/items/get</c>. <c>data</c> is an array of <see cref="LazOrderItem"/>.</summary>
public sealed record GetOrderItemsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazOrderItem>? Data { get; init; }
}

/// <summary>
/// Single order item returned by <c>/order/items/get</c> and <c>/orders/items/get</c>.
/// Monetary fields stay as <see cref="string"/> (verbatim platform values, often with trailing zeros);
/// integer fields with string wire-form use <see cref="JsonNumberHandling.AllowReadingFromString"/>;
/// booleans use <see cref="StringOrBoolJsonConverter"/> (Lazada wires them as "true"/"True"/"0"/"1"/native).
/// </summary>
public sealed record LazOrderItem
{
    // ---- identifiers ----
    [JsonPropertyName("order_item_id")]   public string? OrderItemId { get; init; }
    [JsonPropertyName("order_id")]        public string? OrderId { get; init; }
    [JsonPropertyName("package_id")]      public string? PackageId { get; init; }
    [JsonPropertyName("purchase_order_id")] public string? PurchaseOrderId { get; init; }
    [JsonPropertyName("purchase_order_number")] public string? PurchaseOrderNumber { get; init; }
    [JsonPropertyName("buyer_id")]        public string? BuyerId { get; init; }
    [JsonPropertyName("shop_id")]         public string? ShopId { get; init; }
    [JsonPropertyName("reverse_order_id")] public string? ReverseOrderId { get; init; }
    [JsonPropertyName("invoice_number")]  public string? InvoiceNumber { get; init; }

    // ---- product ----
    [JsonPropertyName("sku")]             public string? Sku { get; init; }
    [JsonPropertyName("sku_id")]          public string? SkuId { get; init; }
    [JsonPropertyName("shop_sku")]        public string? ShopSku { get; init; }
    [JsonPropertyName("product_id")]      public string? ProductId { get; init; }
    [JsonPropertyName("product_main_image")] public string? ProductMainImage { get; init; }
    [JsonPropertyName("product_detail_url")] public string? ProductDetailUrl { get; init; }
    [JsonPropertyName("name")]            public string? Name { get; init; }
    [JsonPropertyName("variation")]       public string? Variation { get; init; }

    // ---- money (verbatim strings, often "0.00") ----
    [JsonPropertyName("currency")]                       public string? Currency { get; init; }
    [JsonPropertyName("item_price")]                     public string? ItemPrice { get; init; }
    [JsonPropertyName("paid_price")]                     public string? PaidPrice { get; init; }
    [JsonPropertyName("supply_price")]                   public string? SupplyPrice { get; init; }
    [JsonPropertyName("supply_price_currency")]          public string? SupplyPriceCurrency { get; init; }
    [JsonPropertyName("tax_amount")]                     public string? TaxAmount { get; init; }
    [JsonPropertyName("wallet_credits")]                 public string? WalletCredits { get; init; }
    [JsonPropertyName("voucher_amount")]                 public string? VoucherAmount { get; init; }
    [JsonPropertyName("voucher_seller")]                 public string? VoucherSeller { get; init; }
    [JsonPropertyName("voucher_platform")]               public string? VoucherPlatform { get; init; }
    [JsonPropertyName("voucher_seller_lpi")]             public string? VoucherSellerLpi { get; init; }
    [JsonPropertyName("voucher_platform_lpi")]           public string? VoucherPlatformLpi { get; init; }
    [JsonPropertyName("voucher_code")]                   public string? VoucherCode { get; init; }
    [JsonPropertyName("voucher_code_seller")]            public string? VoucherCodeSeller { get; init; }
    [JsonPropertyName("voucher_code_platform")]          public string? VoucherCodePlatform { get; init; }
    [JsonPropertyName("shipping_amount")]                public string? ShippingAmount { get; init; }
    [JsonPropertyName("shipping_fee_original")]          public string? ShippingFeeOriginal { get; init; }
    [JsonPropertyName("shipping_fee_discount_seller")]   public string? ShippingFeeDiscountSeller { get; init; }
    [JsonPropertyName("shipping_fee_discount_platform")] public string? ShippingFeeDiscountPlatform { get; init; }
    [JsonPropertyName("shipping_service_cost")]          public string? ShippingServiceCost { get; init; }

    // ---- shipping / fulfillment ----
    [JsonPropertyName("shipping_type")]            public string? ShippingType { get; init; }
    [JsonPropertyName("shipping_provider_type")]   public string? ShippingProviderType { get; init; }
    [JsonPropertyName("shipment_provider")]        public string? ShipmentProvider { get; init; }
    [JsonPropertyName("warehouse_code")]           public string? WarehouseCode { get; init; }
    [JsonPropertyName("tracking_code")]            public string? TrackingCode { get; init; }
    [JsonPropertyName("tracking_code_pre")]        public string? TrackingCodePre { get; init; }
    [JsonPropertyName("fulfillment_sla")]          public string? FulfillmentSla { get; init; }
    [JsonPropertyName("promised_shipping_time")]   public string? PromisedShippingTime { get; init; }
    [JsonPropertyName("sla_time_stamp")]           public string? SlaTimeStamp { get; init; }
    [JsonPropertyName("priority_fulfillment_tag")] public string? PriorityFulfillmentTag { get; init; }

    // ---- status / lifecycle ----
    [JsonPropertyName("status")]            public string? Status { get; init; }
    [JsonPropertyName("stage_pay_status")]  public string? StagePayStatus { get; init; }

    [JsonPropertyName("return_status")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? ReturnStatus { get; init; }

    [JsonPropertyName("order_type")]              public string? OrderType { get; init; }
    [JsonPropertyName("order_flag")]              public string? OrderFlag { get; init; }
    [JsonPropertyName("biz_group")]               public string? BizGroup { get; init; }
    [JsonPropertyName("delivery_option_sof")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? DeliveryOptionSof { get; init; }

    [JsonPropertyName("reason")]          public string? Reason { get; init; }
    [JsonPropertyName("reason_detail")]   public string? ReasonDetail { get; init; }

    [JsonPropertyName("cancel_trigger_time")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? CancelTriggerTime { get; init; }

    [JsonPropertyName("cancel_return_initiator")] public string? CancelReturnInitiator { get; init; }

    // ---- booleans (mixed string forms) ----
    [JsonPropertyName("is_digital")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool IsDigital { get; init; }

    [JsonPropertyName("is_fbl")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool IsFbl { get; init; }

    [JsonPropertyName("is_reroute")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool IsReroute { get; init; }

    [JsonPropertyName("is_cancel_pending")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool IsCancelPending { get; init; }

    [JsonPropertyName("need_cancel_confirm")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool NeedCancelConfirm { get; init; }

    [JsonPropertyName("can_escalate_pickup")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool CanEscalatePickup { get; init; }

    // Lazada wires this with two spellings: '/order/items/get' uses 'show_giftwrapping_tag'
    // (no underscore), '/orders/items/get' uses 'show_gift_wrapping_tag' (with underscore).
    // Capture both; ShowGiftWrappingTag is the convenience accessor.
    [JsonPropertyName("show_giftwrapping_tag")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool ShowGiftWrappingTagCompact { get; init; }

    [JsonPropertyName("show_gift_wrapping_tag")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool ShowGiftWrappingTagUnderscored { get; init; }

    /// <summary>Convenience accessor — true if either spelling is true.</summary>
    [JsonIgnore]
    public bool ShowGiftWrappingTag => ShowGiftWrappingTagCompact || ShowGiftWrappingTagUnderscored;

    [JsonPropertyName("show_personalization_tag")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool ShowPersonalizationTag { get; init; }

    [JsonPropertyName("mp3_order")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Mp3Order { get; init; }

    [JsonPropertyName("semi_managed")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool SemiManaged { get; init; }

    // ---- gifts / personalization ----
    [JsonPropertyName("gift_wrapping")]   public string? GiftWrapping { get; init; }
    [JsonPropertyName("personalization")] public string? Personalization { get; init; }

    // ---- schedule / payment ----
    [JsonPropertyName("payment_time")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? PaymentTime { get; init; }

    [JsonPropertyName("schedule_delivery_start_timeslot")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ScheduleDeliveryStartTimeslot { get; init; }

    [JsonPropertyName("schedule_delivery_end_timeslot")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ScheduleDeliveryEndTimeslot { get; init; }

    [JsonPropertyName("digital_delivery_info")] public string? DigitalDeliveryInfo { get; init; }

    // ---- pickup store ----
    [JsonPropertyName("pick_up_store_info")] public LazPickUpStoreInfo? PickUpStoreInfo { get; init; }

    // ---- misc / timestamps ----
    [JsonPropertyName("created_at")]      public string? CreatedAt { get; init; }
    [JsonPropertyName("updated_at")]      public string? UpdatedAt { get; init; }
    [JsonPropertyName("extra_attributes")] public string? ExtraAttributes { get; init; }
}

/// <summary>Click-and-collect pickup store metadata.</summary>
public sealed record LazPickUpStoreInfo
{
    [JsonPropertyName("pick_up_store_name")]      public string? Name { get; init; }
    [JsonPropertyName("pick_up_store_address")]   public string? Address { get; init; }
    [JsonPropertyName("pick_up_store_code")]      public string? Code { get; init; }
    [JsonPropertyName("pick_up_store_open_hour")] public IReadOnlyList<string>? OpenHour { get; init; }
}
