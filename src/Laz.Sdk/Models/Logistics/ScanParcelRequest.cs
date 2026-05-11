using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Logistics;

/// <summary>Request for <c>/dop/scan</c> (DOP Scan Parcel).</summary>
public sealed record ScanParcelRequest
{
    public required string CageNumber { get; init; }
    public required string TrackingNumber { get; init; }
}

/// <summary>Response for <c>/dop/scan</c>.</summary>
public sealed record ScanParcelResponse
{
    [JsonPropertyName("code")]           public string? Code { get; init; }
    [JsonPropertyName("type")]           public string? Type { get; init; }
    [JsonPropertyName("message")]        public string? Message { get; init; }
    [JsonPropertyName("request_id")]     public string? RequestId { get; init; }
    [JsonPropertyName("trackingNumber")] public string? TrackingNumber { get; init; }
}

/// <summary>Request for <c>/stations/dop/scan</c>. Same params as <see cref="ScanParcelRequest"/>.</summary>
public sealed record StationDopScanRequest
{
    public required string CageNumber { get; init; }
    public required string TrackingNumber { get; init; }
}

/// <summary>Response for <c>/stations/dop/scan</c>.</summary>
public sealed record StationDopScanResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }

    [JsonPropertyName("success")]
    [JsonConverter(typeof(Laz.Sdk.Json.StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("data")]  public StationDopScanData? Data { get; init; }
    [JsonPropertyName("error")] public StationDopScanError? Error { get; init; }
}

public sealed record StationDopScanData
{
    [JsonPropertyName("trackingNumber")] public string? TrackingNumber { get; init; }
}

public sealed record StationDopScanError
{
    [JsonPropertyName("errorCode")] public string? ErrorCode { get; init; }
}
