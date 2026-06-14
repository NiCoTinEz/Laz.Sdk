using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Store;

/// <summary>Response for <c>/store/custom/page/get</c>.</summary>
public sealed record GetStoreCustomPageResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public StoreCustomPageData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object for <see cref="GetStoreCustomPageResponse"/>.</summary>
public sealed record StoreCustomPageData
{
    [JsonPropertyName("total")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? Total { get; init; }

    [JsonPropertyName("currentPage")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? CurrentPage { get; init; }

    [JsonPropertyName("pageSize")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int? PageSize { get; init; }

    [JsonPropertyName("list")]
    public IReadOnlyList<StoreCustomPage>? List { get; init; }
}

/// <summary>A single store custom page.</summary>
public sealed record StoreCustomPage
{
    [JsonPropertyName("page_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? PageId { get; init; }

    [JsonPropertyName("page_name")]  public string? PageName { get; init; }
    [JsonPropertyName("page_type")]  public string? PageType { get; init; }
    [JsonPropertyName("page_url")]   public string? PageUrl { get; init; }
    [JsonPropertyName("created_at")] public string? CreatedAt { get; init; }
    [JsonPropertyName("updated_at")] public string? UpdatedAt { get; init; }
}
