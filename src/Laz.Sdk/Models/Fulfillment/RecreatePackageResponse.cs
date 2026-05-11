using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>
/// Response envelope for <c>/order/package/repack</c>. Same per-package outcome shape
/// as <see cref="ReadyToShipV2Response"/>.
/// </summary>
public sealed record RecreatePackageResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("result")]     public RecreatePackageResult? Result { get; init; }
}

public sealed record RecreatePackageResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }
    [JsonPropertyName("error_msg")]  public string? ErrorMsg { get; init; }
    [JsonPropertyName("data")]       public RecreatePackageData? Data { get; init; }
}

public sealed record RecreatePackageData
{
    /// <summary>Reuses <see cref="ReadyToShipPackageResult"/> — identical per-package payload.</summary>
    [JsonPropertyName("packages")] public IReadOnlyList<ReadyToShipPackageResult>? Packages { get; init; }
}
