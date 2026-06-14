using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Products;

/// <summary>Request for <c>/images/set</c>.</summary>
public sealed record SetImagesRequest
{
    /// <summary>Product item ID.</summary>
    [JsonPropertyName("item_id")]
    public long ItemId { get; init; }

    /// <summary>List of image URLs to set.</summary>
    [JsonPropertyName("images")]
    public IReadOnlyList<string>? Images { get; init; }
}
