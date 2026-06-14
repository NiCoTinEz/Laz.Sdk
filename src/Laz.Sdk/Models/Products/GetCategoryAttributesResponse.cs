using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/category/attributes/get</c>.</summary>
public sealed record GetCategoryAttributesResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazCategoryAttribute>? Data { get; init; }
}

/// <summary>Category attribute definition.</summary>
public sealed record LazCategoryAttribute
{
    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long Id { get; init; }

    [JsonPropertyName("name")]         public string? Name { get; init; }
    [JsonPropertyName("label")]        public string? Label { get; init; }
    [JsonPropertyName("attribute_type")] public string? AttributeType { get; init; }
    [JsonPropertyName("input_type")]   public string? InputType { get; init; }
    [JsonPropertyName("options")]      public IReadOnlyList<LazAttributeOption>? Options { get; init; }
    [JsonPropertyName("is_mandatory")]
    [JsonConverter(typeof(Json.StringOrBoolJsonConverter))]
    public bool IsMandatory { get; init; }

    [JsonPropertyName("is_sale_prop")]
    [JsonConverter(typeof(Json.StringOrBoolJsonConverter))]
    public bool IsSaleProp { get; init; }

    [JsonPropertyName("is_variant")]
    [JsonConverter(typeof(Json.StringOrBoolJsonConverter))]
    public bool IsVariant { get; init; }

    [JsonPropertyName("is_configurable")]
    [JsonConverter(typeof(Json.StringOrBoolJsonConverter))]
    public bool IsConfigurable { get; init; }

    [JsonPropertyName("is_multilingual")]
    [JsonConverter(typeof(Json.StringOrBoolJsonConverter))]
    public bool IsMultilingual { get; init; }
}

/// <summary>Attribute option value.</summary>
public sealed record LazAttributeOption
{
    [JsonPropertyName("name")]  public string? Name { get; init; }
    [JsonPropertyName("label")] public string? Label { get; init; }
}
