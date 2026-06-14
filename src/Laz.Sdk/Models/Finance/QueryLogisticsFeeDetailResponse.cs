using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Finance;

/// <summary>Response envelope for <c>/lbs/slb/queryLogisticsFeeDetail</c>.</summary>
public sealed record QueryLogisticsFeeDetailResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public LogisticsFeePage? Data { get; init; }
}

/// <summary>Paginated logistics fee details.</summary>
public sealed record LogisticsFeePage
{
    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Total { get; init; }

    [JsonPropertyName("currentPage")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int CurrentPage { get; init; }

    [JsonPropertyName("pageSize")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int PageSize { get; init; }

    [JsonPropertyName("list")]
    public IReadOnlyList<LogisticsFeeDetail>? List { get; init; }
}

/// <summary>A single logistics fee detail record.</summary>
public sealed record LogisticsFeeDetail
{
    [JsonPropertyName("order_number")]
    public string? OrderNumber { get; init; }

    [JsonPropertyName("order_item_id")]
    public string? OrderItemId { get; init; }

    [JsonPropertyName("shipment_provider")]
    public string? ShipmentProvider { get; init; }

    [JsonPropertyName("tracking_number")]
    public string? TrackingNumber { get; init; }

    [JsonPropertyName("fee_type")]
    public string? FeeType { get; init; }

    [JsonPropertyName("fee_amount")]
    public string? FeeAmount { get; init; }

    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    [JsonPropertyName("charge_time")]
    public string? ChargeTime { get; init; }

    [JsonPropertyName("remark")]
    public string? Remark { get; init; }
}
