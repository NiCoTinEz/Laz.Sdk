using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Orders;

/// <summary>Response envelope for <c>/orders/get</c>.</summary>
public sealed record GetOrdersResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public GetOrdersData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <c>/orders/get</c>.</summary>
public sealed record GetOrdersData
{
    /// <summary>Number of orders in this page.</summary>
    [JsonPropertyName("count")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Count { get; init; }

    /// <summary>Total number of orders matching the filter.</summary>
    [JsonPropertyName("countTotal")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int CountTotal { get; init; }

    /// <summary>The page of orders.</summary>
    [JsonPropertyName("orders")]
    public IReadOnlyList<LazOrder>? Orders { get; init; }
}

/// <summary>
/// Order entity surfaced by <c>/orders/get</c>. Lazada returns money + numeric fields as strings;
/// monetary fields are exposed as <see cref="string"/> verbatim; pure numeric fields use
/// <see cref="JsonNumberHandling.AllowReadingFromString"/>.
/// </summary>
public sealed record LazOrder
{
    [JsonPropertyName("order_id")]                       public string? OrderId { get; init; }
    [JsonPropertyName("order_number")]                   public string? OrderNumber { get; init; }
    [JsonPropertyName("created_at")]                     public string? CreatedAt { get; init; }
    [JsonPropertyName("updated_at")]                     public string? UpdatedAt { get; init; }
    [JsonPropertyName("address_updated_at")]             public string? AddressUpdatedAt { get; init; }

    [JsonPropertyName("customer_first_name")]            public string? CustomerFirstName { get; init; }
    [JsonPropertyName("customer_last_name")]             public string? CustomerLastName { get; init; }
    [JsonPropertyName("buyer_note")]                     public string? BuyerNote { get; init; }
    [JsonPropertyName("remarks")]                        public string? Remarks { get; init; }

    [JsonPropertyName("price")]                          public string? Price { get; init; }
    [JsonPropertyName("payment_method")]                 public string? PaymentMethod { get; init; }
    [JsonPropertyName("voucher")]                        public string? Voucher { get; init; }
    [JsonPropertyName("voucher_platform")]               public string? VoucherPlatform { get; init; }
    [JsonPropertyName("voucher_seller")]                 public string? VoucherSeller { get; init; }
    [JsonPropertyName("voucher_code")]                   public string? VoucherCode { get; init; }
    [JsonPropertyName("shipping_fee")]                   public string? ShippingFee { get; init; }
    [JsonPropertyName("shipping_fee_original")]          public string? ShippingFeeOriginal { get; init; }
    [JsonPropertyName("shipping_fee_discount_seller")]   public string? ShippingFeeDiscountSeller { get; init; }
    [JsonPropertyName("shipping_fee_discount_platform")] public string? ShippingFeeDiscountPlatform { get; init; }

    [JsonPropertyName("items_count")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int ItemsCount { get; init; }

    [JsonPropertyName("statuses")]                       public IReadOnlyList<string>? Statuses { get; init; }
    [JsonPropertyName("warehouse_code")]                 public string? WarehouseCode { get; init; }
    [JsonPropertyName("delivery_info")]                  public string? DeliveryInfo { get; init; }
    [JsonPropertyName("promised_shipping_times")]        public string? PromisedShippingTimes { get; init; }
    [JsonPropertyName("gift_option")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool GiftOption { get; init; }

    [JsonPropertyName("gift_message")]                   public string? GiftMessage { get; init; }
    [JsonPropertyName("is_cancel_pending")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool IsCancelPending { get; init; }

    [JsonPropertyName("need_cancel_confirm")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool NeedCancelConfirm { get; init; }

    [JsonPropertyName("national_registration_number")]   public string? NationalRegistrationNumber { get; init; }
    [JsonPropertyName("tax_code")]                       public string? TaxCode { get; init; }
    [JsonPropertyName("branch_number")]                  public string? BranchNumber { get; init; }
    [JsonPropertyName("extra_attributes")]               public string? ExtraAttributes { get; init; }

    [JsonPropertyName("address_billing")]                public LazAddress? AddressBilling { get; init; }
    [JsonPropertyName("address_shipping")]               public LazAddress? AddressShipping { get; init; }
    [JsonPropertyName("recipient_info")]                 public LazRecipientInfo? RecipientInfo { get; init; }
}

/// <summary>Billing / shipping address as returned by Lazada.</summary>
public sealed record LazAddress
{
    [JsonPropertyName("first_name")]       public string? FirstName { get; init; }
    [JsonPropertyName("last_name")]        public string? LastName { get; init; }
    [JsonPropertyName("phone")]            public string? Phone { get; init; }
    [JsonPropertyName("phone2")]           public string? Phone2 { get; init; }
    [JsonPropertyName("address1")]         public string? Address1 { get; init; }
    [JsonPropertyName("address2")]         public string? Address2 { get; init; }
    [JsonPropertyName("address3")]         public string? Address3 { get; init; }
    [JsonPropertyName("address4")]         public string? Address4 { get; init; }
    [JsonPropertyName("address5")]         public string? Address5 { get; init; }
    [JsonPropertyName("addressDsitrict")]  public string? AddressDistrict { get; init; }
    [JsonPropertyName("city")]             public string? City { get; init; }
    [JsonPropertyName("post_code")]        public string? PostCode { get; init; }
    [JsonPropertyName("country")]          public string? Country { get; init; }
}

/// <summary>Country-specific recipient identifiers (Indonesia, Vietnam, etc.).</summary>
public sealed record LazRecipientInfo
{
    [JsonPropertyName("identify_no")]   public string? IdentifyNo { get; init; }
    [JsonPropertyName("passport_no")]   public string? PassportNo { get; init; }
    [JsonPropertyName("detail_address")] public string? DetailAddress { get; init; }
}
