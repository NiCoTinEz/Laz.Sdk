using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Response envelope for <c>/order/package/rts</c>.</summary>
public sealed record ReadyToShipV2Response
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("result")]     public ReadyToShipV2Result? Result { get; init; }
}

public sealed record ReadyToShipV2Result
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }
    [JsonPropertyName("error_msg")]  public string? ErrorMsg { get; init; }
    [JsonPropertyName("data")]       public ReadyToShipV2Data? Data { get; init; }
}

public sealed record ReadyToShipV2Data
{
    [JsonPropertyName("packages")] public IReadOnlyList<ReadyToShipPackageResult>? Packages { get; init; }
}

public sealed record ReadyToShipPackageResult
{
    [JsonPropertyName("package_id")]    public string? PackageId { get; init; }
    [JsonPropertyName("msg")]           public string? Msg { get; init; }

    [JsonPropertyName("item_err_code")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? ItemErrCode { get; init; }

    [JsonPropertyName("retry")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Retry { get; init; }
}
