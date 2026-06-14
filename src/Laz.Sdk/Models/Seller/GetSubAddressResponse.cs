using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/address/sub/get</c>.</summary>
public sealed record GetSubAddressResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<SubAddress>? Data { get; init; }
}

/// <summary>A sub-address entry.</summary>
public sealed record SubAddress
{
    [JsonPropertyName("address_id")]   public string? AddressId { get; init; }
    [JsonPropertyName("address_name")] public string? AddressName { get; init; }
    [JsonPropertyName("address_line")] public string? AddressLine { get; init; }
    [JsonPropertyName("city")]         public string? City { get; init; }
    [JsonPropertyName("district")]     public string? District { get; init; }
    [JsonPropertyName("state")]        public string? State { get; init; }
    [JsonPropertyName("country")]      public string? Country { get; init; }
    [JsonPropertyName("zip_code")]     public string? ZipCode { get; init; }
    [JsonPropertyName("phone")]        public string? Phone { get; init; }
}
