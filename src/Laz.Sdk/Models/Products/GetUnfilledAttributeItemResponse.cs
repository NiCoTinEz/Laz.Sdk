using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/unfilled/attribute/item/get</c>.</summary>
public sealed record GetUnfilledAttributeItemResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazUnfilledAttributeItem>? Data { get; init; }
}

/// <summary>Unfilled attribute item.</summary>
public sealed record LazUnfilledAttributeItem
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("seller_sku")] public string? SellerSku { get; init; }
    [JsonPropertyName("attributes")] public string? Attributes { get; init; }
}
