using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Seller;

/// <summary>Response envelope for <c>/seller/metrics/get</c>.</summary>
public sealed record GetSellerMetricsResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public SellerMetricsData? Data { get; init; }
}

/// <summary>Seller metrics information.</summary>
public sealed record SellerMetricsData
{
    [JsonPropertyName("main_category_name")] public string? MainCategoryName { get; init; }
    [JsonPropertyName("seller_id")]          public string? SellerId { get; init; }

    [JsonPropertyName("response_rate")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public double? ResponseRate { get; init; }

    [JsonPropertyName("response_time")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public double? ResponseTime { get; init; }

    [JsonPropertyName("ship_on_time")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public double? ShipOnTime { get; init; }

    [JsonPropertyName("main_category_id")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? MainCategoryId { get; init; }

    [JsonPropertyName("positive_seller_rating")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public double? PositiveSellerRating { get; init; }
}
