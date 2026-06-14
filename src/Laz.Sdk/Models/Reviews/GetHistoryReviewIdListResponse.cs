using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Reviews;

/// <summary>Response for <c>/review/seller/history/list</c>.</summary>
public sealed record GetHistoryReviewIdListResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public GetHistoryReviewIdListData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="GetHistoryReviewIdListResponse"/>.</summary>
public sealed record GetHistoryReviewIdListData
{
    [JsonPropertyName("id_list")]
    public IReadOnlyList<long>? IdList { get; init; }

    [JsonPropertyName("current")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Current { get; init; }

    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Total { get; init; }

    [JsonPropertyName("page_size")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? PageSize { get; init; }
}
