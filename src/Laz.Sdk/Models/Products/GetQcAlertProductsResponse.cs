using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/qc/alert/products/get</c>.</summary>
public sealed record GetQcAlertProductsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public GetQcAlertProductsData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for QC alert products.</summary>
public sealed record GetQcAlertProductsData
{
    [JsonPropertyName("alert_products")]
    public IReadOnlyList<LazQcAlertProduct>? AlertProducts { get; init; }
}

/// <summary>QC alert product.</summary>
public sealed record LazQcAlertProduct
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("seller_sku")] public string? SellerSku { get; init; }
    [JsonPropertyName("reason")]     public string? Reason { get; init; }
    [JsonPropertyName("alert")]      public string? Alert { get; init; }
}
