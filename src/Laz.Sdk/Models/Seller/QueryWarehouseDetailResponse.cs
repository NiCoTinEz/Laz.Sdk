using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/warehouse/detail/query</c>.</summary>
public sealed record QueryWarehouseDetailResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public WarehouseDetailData? Data { get; init; }
}

/// <summary>Warehouse detail information.</summary>
public sealed record WarehouseDetailData
{
    [JsonPropertyName("warehouse_id")]      public string? WarehouseId { get; init; }
    [JsonPropertyName("warehouse_name")]    public string? WarehouseName { get; init; }
    [JsonPropertyName("warehouse_code")]    public string? WarehouseCode { get; init; }
    [JsonPropertyName("address")]           public string? Address { get; init; }
    [JsonPropertyName("city")]              public string? City { get; init; }
    [JsonPropertyName("district")]          public string? District { get; init; }
    [JsonPropertyName("state")]             public string? State { get; init; }
    [JsonPropertyName("country")]           public string? Country { get; init; }
    [JsonPropertyName("zip_code")]          public string? ZipCode { get; init; }
    [JsonPropertyName("contact_person")]    public string? ContactPerson { get; init; }
    [JsonPropertyName("contact_phone")]     public string? ContactPhone { get; init; }
    [JsonPropertyName("status")]            public string? Status { get; init; }
    [JsonPropertyName("warehouse_type")]    public string? WarehouseType { get; init; }
}
