using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.CrossBorder;

/// <summary>Response for <c>/product/global/create</c>.</summary>
public sealed record CreateGlobalProductResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public CreateGlobalProductData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="CreateGlobalProductResponse"/>.</summary>
public sealed record CreateGlobalProductData
{
    [JsonPropertyName("sku_list")]
    public IReadOnlyList<GlobalSkuInfo>? SkuList { get; init; }
}

/// <summary>SKU info returned from global product creation.</summary>
public sealed record GlobalSkuInfo
{
    [JsonPropertyName("seller_sku")] public string? SellerSku { get; init; }
    [JsonPropertyName("sku_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? SkuId { get; init; }
    [JsonPropertyName("shop_sku")]   public string? ShopSku { get; init; }
}
