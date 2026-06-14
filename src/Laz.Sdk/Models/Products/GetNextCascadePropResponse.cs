using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/next/cascade/prop/get</c>.</summary>
public sealed record GetNextCascadePropResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazCascadeProp>? Data { get; init; }
}

/// <summary>Cascade property.</summary>
public sealed record LazCascadeProp
{
    [JsonPropertyName("prop_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long PropId { get; init; }

    [JsonPropertyName("prop_name")]    public string? PropName { get; init; }
    [JsonPropertyName("prop_unit")]    public string? PropUnit { get; init; }
    [JsonPropertyName("is_key_prop")]
    [JsonConverter(typeof(Json.StringOrBoolJsonConverter))]
    public bool IsKeyProp { get; init; }

    [JsonPropertyName("is_mandatory")]
    [JsonConverter(typeof(Json.StringOrBoolJsonConverter))]
    public bool IsMandatory { get; init; }

    [JsonPropertyName("is_sale_prop")]
    [JsonConverter(typeof(Json.StringOrBoolJsonConverter))]
    public bool IsSaleProp { get; init; }

    [JsonPropertyName("input_type")]   public string? InputType { get; init; }
    [JsonPropertyName("options")]      public IReadOnlyList<LazCascadePropOption>? Options { get; init; }
}

/// <summary>Cascade property option.</summary>
public sealed record LazCascadePropOption
{
    [JsonPropertyName("option_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long OptionId { get; init; }

    [JsonPropertyName("name")] public string? Name { get; init; }
}
