using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/content/score/get</c>.</summary>
public sealed record GetProductContentScoreResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public GetProductContentScoreData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for product content score.</summary>
public sealed record GetProductContentScoreData
{
    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("score")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Score { get; init; }

    [JsonPropertyName("details")] public string? Details { get; init; }
}
