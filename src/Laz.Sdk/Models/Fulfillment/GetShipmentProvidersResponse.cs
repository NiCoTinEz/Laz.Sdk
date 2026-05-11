using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Response envelope for <c>/order/shipment/providers/get</c>.</summary>
public sealed record GetShipmentProvidersResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("result")]     public GetShipmentProvidersResult? Result { get; init; }
}

/// <summary>Inner <c>result</c> object — carries its own success / error / data triple.</summary>
public sealed record GetShipmentProvidersResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }
    [JsonPropertyName("error_msg")]  public string? ErrorMsg { get; init; }
    [JsonPropertyName("data")]       public GetShipmentProvidersData? Data { get; init; }
}

/// <summary>The shipping-provider payload itself.</summary>
public sealed record GetShipmentProvidersData
{
    [JsonPropertyName("platform_default")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int PlatformDefault { get; init; }

    [JsonPropertyName("shipping_allocate_type")]
    public string? ShippingAllocateType { get; init; }

    [JsonPropertyName("shipment_providers")]
    public IReadOnlyList<ShipmentProviderInfo>? ShipmentProviders { get; init; }
}

/// <summary>Single shipping provider entry.</summary>
public sealed record ShipmentProviderInfo
{
    [JsonPropertyName("name")]          public string? Name { get; init; }
    [JsonPropertyName("provider_code")] public string? ProviderCode { get; init; }
}
