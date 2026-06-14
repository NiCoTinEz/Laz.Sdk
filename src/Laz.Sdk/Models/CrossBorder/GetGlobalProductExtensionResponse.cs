using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.CrossBorder;

/// <summary>Response for <c>/product/global/extension</c>.</summary>
public sealed record GetGlobalProductExtensionResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<GlobalProductExtension>? Data { get; init; }
}

/// <summary>A single global product extension entry.</summary>
public sealed record GlobalProductExtension
{
    [JsonPropertyName("global_item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? GlobalItemId { get; init; }

    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("extensions")]
    public IReadOnlyList<GlobalExtensionDetail>? Extensions { get; init; }
}

/// <summary>Extension detail for a global product.</summary>
public sealed record GlobalExtensionDetail
{
    [JsonPropertyName("name")]  public string? Name { get; init; }
    [JsonPropertyName("value")] public string? Value { get; init; }
}
