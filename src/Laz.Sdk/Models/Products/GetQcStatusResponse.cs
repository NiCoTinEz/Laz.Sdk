using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/qc/status/get</c>.</summary>
public sealed record GetQcStatusResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazQcStatus>? Data { get; init; }
}

/// <summary>QC status for a product.</summary>
public sealed record LazQcStatus
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("seller_sku")]  public string? SellerSku { get; init; }
    [JsonPropertyName("reason")]      public string? Reason { get; init; }
    [JsonPropertyName("status")]      public string? Status { get; init; }
}
