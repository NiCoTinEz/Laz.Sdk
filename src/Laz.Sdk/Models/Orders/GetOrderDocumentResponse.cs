using System.Text.Json.Serialization;

namespace Laz.Sdk.Models.Orders;

/// <summary>
/// Response envelope from <c>/order/document/get</c>.
/// </summary>
public sealed record GetOrderDocumentResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("data")]       public OrderDocumentData? Data { get; init; }
}

/// <summary>Inner <c>data</c> object.</summary>
public sealed record OrderDocumentData
{
    [JsonPropertyName("document")] public OrderDocument? Document { get; init; }
}

/// <summary>
/// Document payload. <see cref="File"/> is Base64-encoded; use <see cref="GetFileBytes"/> to decode.
/// </summary>
public sealed record OrderDocument
{
    [JsonPropertyName("file")]          public string? File { get; init; }
    [JsonPropertyName("mime_type")]     public string? MimeType { get; init; }
    [JsonPropertyName("document_type")] public string? DocumentType { get; init; }

    /// <summary>Decode <see cref="File"/> into raw bytes. Returns an empty array when unavailable.</summary>
    public byte[] GetFileBytes()
        => string.IsNullOrEmpty(File) ? [] : Convert.FromBase64String(File);
}
