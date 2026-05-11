using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Logistics;

/// <summary>Response envelope for <c>/logistic/order/trace</c>.</summary>
public sealed record GetOrderTraceResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("result")]     public OrderTraceResult? Result { get; init; }
}

public sealed record OrderTraceResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("not_success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool NotSuccess { get; init; }

    [JsonPropertyName("repeated")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Repeated { get; init; }

    [JsonPropertyName("retry")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Retry { get; init; }

    [JsonPropertyName("error_code")] public OrderTraceErrorCode? ErrorCode { get; init; }
    [JsonPropertyName("module")]     public IReadOnlyList<OrderTraceModule>? Module { get; init; }
}

public sealed record OrderTraceErrorCode
{
    [JsonPropertyName("displayMessage")] public string? DisplayMessage { get; init; }
}

public sealed record OrderTraceModule
{
    [JsonPropertyName("ofc_order_id")]            public string? OfcOrderId { get; init; }
    [JsonPropertyName("warehouse_detail_info")]   public object? WarehouseDetailInfo { get; init; }
    [JsonPropertyName("package_detail_info_list")] public IReadOnlyList<OrderTracePackage>? PackageDetailInfoList { get; init; }
}

public sealed record OrderTracePackage
{
    [JsonPropertyName("ofc_package_id")]           public string? OfcPackageId { get; init; }
    [JsonPropertyName("tracking_number")]          public string? TrackingNumber { get; init; }
    [JsonPropertyName("order_line_info_list")]     public object? OrderLineInfoList { get; init; }
    [JsonPropertyName("logistic_detail_info_list")] public IReadOnlyList<OrderTraceEvent>? LogisticDetailInfoList { get; init; }
}

/// <summary>Single tracking event entry.</summary>
public sealed record OrderTraceEvent
{
    [JsonPropertyName("status_code")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? StatusCode { get; init; }

    [JsonPropertyName("detail_type")]            public string? DetailType { get; init; }
    [JsonPropertyName("title")]                  public string? Title { get; init; }
    [JsonPropertyName("description")]            public string? Description { get; init; }
    [JsonPropertyName("icon")]                   public string? Icon { get; init; }
    [JsonPropertyName("package_location_name")]  public string? PackageLocationName { get; init; }
    [JsonPropertyName("event_date")]             public string? EventDate { get; init; }

    /// <summary>Event time as unix milliseconds.</summary>
    [JsonPropertyName("event_time")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? EventTime { get; init; }

    [JsonPropertyName("receive_time")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ReceiveTime { get; init; }

    [JsonPropertyName("proof_images")] public IReadOnlyList<string>? ProofImages { get; init; }
}
