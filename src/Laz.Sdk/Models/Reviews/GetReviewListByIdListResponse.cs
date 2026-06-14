using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Reviews;

/// <summary>Response for <c>/review/seller/list/v2</c>.</summary>
public sealed record GetReviewListByIdListResponse
{
    [JsonPropertyName("code")]         public string? Code { get; init; }
    [JsonPropertyName("type")]         public string? Type { get; init; }
    [JsonPropertyName("message")]      public string? Message { get; init; }
    [JsonPropertyName("request_id")]   public string? RequestId { get; init; }
    [JsonPropertyName("data")]         public IReadOnlyList<ReviewDetail>? Data { get; init; }
}

/// <summary>Detailed review information.</summary>
public sealed record ReviewDetail
{
    [JsonPropertyName("id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? Id { get; init; }

    [JsonPropertyName("item_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ItemId { get; init; }

    [JsonPropertyName("order_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? OrderId { get; init; }

    [JsonPropertyName("sku_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? SkuId { get; init; }

    [JsonPropertyName("seller_sku")]        public string? SellerSku { get; init; }
    [JsonPropertyName("buyer_name")]        public string? BuyerName { get; init; }
    [JsonPropertyName("title")]             public string? Title { get; init; }
    [JsonPropertyName("content")]           public string? Content { get; init; }
    [JsonPropertyName("rating")]            public int? Rating { get; init; }
    [JsonPropertyName("rating_1")]          public int? Rating1 { get; init; }
    [JsonPropertyName("rating_2")]          public int? Rating2 { get; init; }
    [JsonPropertyName("rating_3")]          public int? Rating3 { get; init; }
    [JsonPropertyName("rating_4")]          public int? Rating4 { get; init; }
    [JsonPropertyName("rating_5")]          public int? Rating5 { get; init; }

    [JsonPropertyName("images")]
    public IReadOnlyList<string>? Images { get; init; }

    [JsonPropertyName("videos")]
    public IReadOnlyList<ReviewVideo>? Videos { get; init; }

    [JsonPropertyName("created_time")]      public string? CreatedTime { get; init; }
    [JsonPropertyName("updated_time")]      public string? UpdatedTime { get; init; }
    [JsonPropertyName("status")]            public string? Status { get; init; }
    [JsonPropertyName("reply")]             public string? Reply { get; init; }
}

/// <summary>Video attached to a review.</summary>
public sealed record ReviewVideo
{
    [JsonPropertyName("url")]   public string? Url { get; init; }
    [JsonPropertyName("cover")] public string? Cover { get; init; }
}
