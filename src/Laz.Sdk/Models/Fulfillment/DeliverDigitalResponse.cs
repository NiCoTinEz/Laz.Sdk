using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Response envelope for <c>/order/digital/delivered</c>.</summary>
public sealed record DeliverDigitalResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("result")]     public DeliverDigitalResult? Result { get; init; }
}

public sealed record DeliverDigitalResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    // Note: camelCase here (not snake_case as in the SOF endpoints).
    [JsonPropertyName("errorCode")] public string? ErrorCode { get; init; }
    [JsonPropertyName("errorMsg")]  public string? ErrorMsg { get; init; }
    [JsonPropertyName("data")]      public DeliverDigitalData? Data { get; init; }
}

public sealed record DeliverDigitalData
{
    [JsonPropertyName("orders")] public IReadOnlyList<DigitalDeliveredOrder>? Orders { get; init; }
}

public sealed record DigitalDeliveredOrder
{
    [JsonPropertyName("order_id")]        public string? OrderId { get; init; }
    [JsonPropertyName("order_item_list")] public IReadOnlyList<DigitalDeliveredItem>? OrderItemList { get; init; }
}

public sealed record DigitalDeliveredItem
{
    [JsonPropertyName("order_item_id")] public string? OrderItemId { get; init; }
    [JsonPropertyName("msg")]           public string? Msg { get; init; }

    [JsonPropertyName("item_err_code")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? ItemErrCode { get; init; }

    [JsonPropertyName("retry")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Retry { get; init; }
}
