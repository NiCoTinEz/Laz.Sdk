using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>
/// Shared response envelope for <c>/order/package/sof/delivered</c> and
/// <c>/order/package/sof/failed_delivery</c> — both use the same per-package
/// outcome shape as <see cref="ReadyToShipV2Response"/>.
/// </summary>
public sealed record DbsPackageDeliveryResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("result")]     public DbsPackageDeliveryResult? Result { get; init; }
}

public sealed record DbsPackageDeliveryResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }
    [JsonPropertyName("error_msg")]  public string? ErrorMsg { get; init; }
    [JsonPropertyName("data")]       public DbsPackageDeliveryData? Data { get; init; }
}

public sealed record DbsPackageDeliveryData
{
    [JsonPropertyName("packages")] public IReadOnlyList<ReadyToShipPackageResult>? Packages { get; init; }
}
