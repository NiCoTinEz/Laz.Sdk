using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Seller;

/// <summary>Request for <c>/seller/field/verify</c>.</summary>
public sealed record SellerFieldVerifyRequest
{
    /// <summary>The field name to verify.</summary>
    [JsonPropertyName("field_name")]
    public string? FieldName { get; init; }

    /// <summary>The field value to verify.</summary>
    [JsonPropertyName("field_value")]
    public string? FieldValue { get; init; }

    /// <summary>Additional verification data as JSON string.</summary>
    [JsonPropertyName("extra_data")]
    public string? ExtraData { get; init; }
}
