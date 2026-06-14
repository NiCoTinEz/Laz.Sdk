using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/warehouse/seller/get</c>.</summary>
public sealed record GetWarehouseBySellerIdResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<WarehouseInfo>? Data { get; init; }
}

/// <summary>Warehouse information.</summary>
public sealed record WarehouseInfo
{
    [JsonPropertyName("warehouse_id")]   public string? WarehouseId { get; init; }
    [JsonPropertyName("warehouse_name")] public string? WarehouseName { get; init; }
    [JsonPropertyName("warehouse_code")] public string? WarehouseCode { get; init; }
    [JsonPropertyName("address")]        public string? Address { get; init; }
    [JsonPropertyName("status")]         public string? Status { get; init; }
    [JsonPropertyName("type")]           public string? Type { get; init; }
    [JsonPropertyName("country")]        public string? Country { get; init; }
}
