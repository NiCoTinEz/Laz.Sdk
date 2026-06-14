using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/category/suggestion/get</c>.</summary>
public sealed record GetCategorySuggestionResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazCategorySuggestion>? Data { get; init; }
}

/// <summary>A suggested category.</summary>
public sealed record LazCategorySuggestion
{
    [JsonPropertyName("category_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long CategoryId { get; init; }

    [JsonPropertyName("category_name")] public string? CategoryName { get; init; }
    [JsonPropertyName("leaf")]          public bool? Leaf { get; init; }
    [JsonPropertyName("confidence")]    public string? Confidence { get; init; }
}

/// <summary>Request for <c>/category/suggestion/bulk/get</c>.</summary>
public sealed record GetCategorySuggestionBulkRequest
{
    /// <summary>JSON string containing array of product names.</summary>
    public string? ProductNames { get; init; }

    /// <summary>Language code (e.g. "en_US", "th_TH").</summary>
    public string? Language { get; init; }
}

/// <summary>Response for <c>/category/suggestion/bulk/get</c>.</summary>
public sealed record GetCategorySuggestionBulkResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazBulkCategorySuggestion>? Data { get; init; }
}

/// <summary>Bulk category suggestion item.</summary>
public sealed record LazBulkCategorySuggestion
{
    [JsonPropertyName("product_name")] public string? ProductName { get; init; }
    [JsonPropertyName("categories")]   public IReadOnlyList<LazCategorySuggestion>? Categories { get; init; }
}
