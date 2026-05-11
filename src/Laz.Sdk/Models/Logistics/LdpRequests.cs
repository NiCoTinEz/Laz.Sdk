using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Logistics;

/// <summary>Request for <c>/logistics/ldp/createConsolidationService</c>.</summary>
public sealed record CreateConsolidationServiceRequest
{
    /// <summary>Fulfillment unit codes.</summary>
    public required IReadOnlyList<string> UnitCodes { get; init; }

    /// <summary>
    /// Free-form properties bag. Common keys: <c>sellerGroupName</c>.
    /// Wired as a JSON object literal.
    /// </summary>
    public required IReadOnlyDictionary<string, string> Properties { get; init; }
}

/// <summary>Request for <c>/logistics/ldp/updateLastmile</c>.</summary>
public sealed record UpdateLastMileRequest
{
    public required string UnitCode { get; init; }
    public required string ShippingProviderCode { get; init; }
    public required string TrackingNumber { get; init; }
}

/// <summary>
/// Shared response envelope for the two <c>/logistics/ldp/*</c> endpoints
/// (createConsolidationService + updateLastmile). Both use the same
/// <c>{code, data, success, errorCode, errorMsg, request_id}</c> top-level shape.
/// </summary>
public sealed record LdpOperationResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }

    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("data")]     public string? Data { get; init; }
    [JsonPropertyName("errorCode")] public string? ErrorCode { get; init; }
    [JsonPropertyName("errorMsg")]  public string? ErrorMsg { get; init; }
}
