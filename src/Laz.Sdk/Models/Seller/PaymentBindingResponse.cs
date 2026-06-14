using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/payment/binding</c>.</summary>
public sealed record PaymentBindingResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public PaymentBindingResultData? Data { get; init; }
}

/// <summary>Payment binding result.</summary>
public sealed record PaymentBindingResultData
{
    [JsonPropertyName("success")]    public string? Success { get; init; }
    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }
    [JsonPropertyName("error_msg")]  public string? ErrorMsg { get; init; }
    [JsonPropertyName("binding_id")] public string? BindingId { get; init; }
}
