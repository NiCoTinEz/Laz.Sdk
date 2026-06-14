using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Base response envelope for product endpoints.</summary>
public sealed record BaseProductResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public BaseProductData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="BaseProductResponse"/>.</summary>
public sealed record BaseProductData
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("sku_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? SkuId { get; init; }

    [JsonPropertyName("seller_sku")] public string? SellerSku { get; init; }
}
