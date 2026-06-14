using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/response/get</c>.</summary>
public sealed record GetProductResponseResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public GetProductResponseData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for product response.</summary>
public sealed record GetProductResponseData
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("seller_sku")]   public string? SellerSku { get; init; }
    [JsonPropertyName("response")]     public string? Response { get; init; }
    [JsonPropertyName("status")]       public string? Status { get; init; }
    [JsonPropertyName("reason")]       public string? Reason { get; init; }
}
