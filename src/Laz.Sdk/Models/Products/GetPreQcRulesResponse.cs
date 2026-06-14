using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Response for <c>/product/pre/qc/rules/get</c>.</summary>
public sealed record GetPreQcRulesResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public IReadOnlyList<LazPreQcRule>? Data { get; init; }
}

/// <summary>Pre-QC rule.</summary>
public sealed record LazPreQcRule
{
    [JsonPropertyName("rule_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? RuleId { get; init; }

    [JsonPropertyName("rule_name")] public string? RuleName { get; init; }
    [JsonPropertyName("status")]    public string? Status { get; init; }
}
