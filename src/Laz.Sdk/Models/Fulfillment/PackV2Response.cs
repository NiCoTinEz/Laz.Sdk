using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Response envelope for <c>/order/fulfill/pack</c>.</summary>
public sealed record PackV2Response
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("result")]     public PackV2Result? Result { get; init; }
}

public sealed record PackV2Result
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }
    [JsonPropertyName("error_msg")]  public string? ErrorMsg { get; init; }
    [JsonPropertyName("data")]       public PackV2Data? Data { get; init; }
}

public sealed record PackV2Data
{
    [JsonPropertyName("pack_order_list")] public IReadOnlyList<PackedOrderGroup>? PackOrderList { get; init; }
}

public sealed record PackedOrderGroup
{
    [JsonPropertyName("order_id")]        public string? OrderId { get; init; }
    [JsonPropertyName("order_item_list")] public IReadOnlyList<PackedOrderItemV2>? OrderItemList { get; init; }
}

public sealed record PackedOrderItemV2
{
    [JsonPropertyName("order_item_id")] public string? OrderItemId { get; init; }
    [JsonPropertyName("msg")]           public string? Msg { get; init; }

    [JsonPropertyName("item_err_code")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? ItemErrCode { get; init; }

    [JsonPropertyName("tracking_number")]   public string? TrackingNumber { get; init; }
    [JsonPropertyName("shipment_provider")] public string? ShipmentProvider { get; init; }
    [JsonPropertyName("package_id")]        public string? PackageId { get; init; }

    [JsonPropertyName("retry")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Retry { get; init; }
}
