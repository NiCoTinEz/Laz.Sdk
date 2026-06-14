using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/seller/item/limit/get</c>.</summary>
public sealed record GetSellerItemLimitResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public GetSellerItemLimitData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for seller item limit.</summary>
public sealed record GetSellerItemLimitData
{
    [JsonPropertyName("item_limit")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? ItemLimit { get; init; }

    [JsonPropertyName("item_count")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? ItemCount { get; init; }

    [JsonPropertyName("remaining")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Remaining { get; init; }
}
