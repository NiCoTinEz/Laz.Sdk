using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Request for <c>/size/chart/batch/update</c>.</summary>
public sealed record BatchUpdateSizeChartRequest
{
    /// <summary>Size chart data as a JSON string.</summary>
    [JsonPropertyName("size_chart_list")]
    public string? SizeChartList { get; init; }
}
