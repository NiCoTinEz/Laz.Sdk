using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/category/tree/get</c>.</summary>
public sealed record GetCategoryTreeResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazCategory>? Data { get; init; }
}

/// <summary>Category node in the category tree.</summary>
public sealed record LazCategory
{
    [JsonPropertyName("category_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long CategoryId { get; init; }

    [JsonPropertyName("name")]           public string? Name { get; init; }
    [JsonPropertyName("var")]            public bool? Var { get; init; }
    [JsonPropertyName("leaf")]           public bool? Leaf { get; init; }
    [JsonPropertyName("enabled")]        public bool? Enabled { get; init; }
    [JsonPropertyName("auto_apply")]     public bool? AutoApply { get; init; }
    [JsonPropertyName("barcode_enabled")] public bool? BarcodeEnabled { get; init; }

    [JsonPropertyName("children")]
    public IReadOnlyList<LazCategory>? Children { get; init; }
}
