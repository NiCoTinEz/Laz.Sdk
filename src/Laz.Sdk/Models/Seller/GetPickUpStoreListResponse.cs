using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/rc/store/list/get</c>.</summary>
public sealed record GetPickUpStoreListResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<PickUpStore>? Data { get; init; }
}

/// <summary>A pickup store.</summary>
public sealed record PickUpStore
{
    [JsonPropertyName("store_id")]   public string? StoreId { get; init; }
    [JsonPropertyName("store_name")] public string? StoreName { get; init; }
    [JsonPropertyName("address")]    public string? Address { get; init; }
    [JsonPropertyName("phone")]      public string? Phone { get; init; }
    [JsonPropertyName("city")]       public string? City { get; init; }
    [JsonPropertyName("district")]   public string? District { get; init; }
    [JsonPropertyName("state")]      public string? State { get; init; }
    [JsonPropertyName("country")]    public string? Country { get; init; }
    [JsonPropertyName("zip_code")]   public string? ZipCode { get; init; }
    [JsonPropertyName("latitude")]   public string? Latitude { get; init; }
    [JsonPropertyName("longitude")]  public string? Longitude { get; init; }
    [JsonPropertyName("status")]     public string? Status { get; init; }
}
