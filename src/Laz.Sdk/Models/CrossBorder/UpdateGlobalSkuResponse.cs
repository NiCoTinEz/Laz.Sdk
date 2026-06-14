using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.CrossBorder;

/// <summary>Response for <c>/product/global/sku/update</c>.</summary>
public sealed record UpdateGlobalSkuResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public UpdateGlobalSkuData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="UpdateGlobalSkuResponse"/>.</summary>
public sealed record UpdateGlobalSkuData
{
    [JsonPropertyName("sku_list")]
    public IReadOnlyList<GlobalSkuInfo>? SkuList { get; init; }
}
