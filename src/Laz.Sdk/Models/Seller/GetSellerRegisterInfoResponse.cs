using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/register/info/get</c>.</summary>
public sealed record GetSellerRegisterInfoResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public SellerRegisterInfoData? Data { get; init; }
}

/// <summary>Seller register information.</summary>
public sealed record SellerRegisterInfoData
{
    [JsonPropertyName("seller_id")]      public string? SellerId { get; init; }
    [JsonPropertyName("company_name")]   public string? CompanyName { get; init; }
    [JsonPropertyName("company_reg_no")] public string? CompanyRegNo { get; init; }
    [JsonPropertyName("tax_id")]         public string? TaxId { get; init; }
    [JsonPropertyName("address")]        public string? Address { get; init; }
    [JsonPropertyName("phone")]          public string? Phone { get; init; }
    [JsonPropertyName("email")]          public string? Email { get; init; }
    [JsonPropertyName("country")]        public string? Country { get; init; }
    [JsonPropertyName("status")]         public string? Status { get; init; }
}
