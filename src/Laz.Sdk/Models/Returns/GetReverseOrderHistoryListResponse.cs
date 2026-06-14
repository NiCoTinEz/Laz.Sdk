using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Returns;

/// <summary>Response envelope for <c>/order/reverse/return/history/list</c>.</summary>
public sealed record GetReverseOrderHistoryListResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public ReverseOrderHistoryListData? Data { get; init; }
}

/// <summary>Paginated history list data.</summary>
public sealed record ReverseOrderHistoryListData
{
    [JsonPropertyName("history_list")] public IReadOnlyList<ReverseOrderHistory>? HistoryList { get; init; }

    [JsonPropertyName("page_size")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int PageSize { get; init; }

    [JsonPropertyName("page_number")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int PageNumber { get; init; }

    [JsonPropertyName("total_count")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int TotalCount { get; init; }
}

/// <summary>A single history entry for a reverse order line.</summary>
public sealed record ReverseOrderHistory
{
    [JsonPropertyName("reverse_order_line_id")] public string? ReverseOrderLineId { get; init; }
    [JsonPropertyName("action")]                public string? Action { get; init; }
    [JsonPropertyName("operator")]              public string? Operator { get; init; }
    [JsonPropertyName("operator_type")]         public string? OperatorType { get; init; }
    [JsonPropertyName("remark")]                public string? Remark { get; init; }
    [JsonPropertyName("created_at")]            public string? CreatedAt { get; init; }
}
