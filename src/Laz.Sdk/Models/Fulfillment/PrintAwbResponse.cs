using System.Text.Json.Serialization;
using Laz.Sdk.Json;

namespace Laz.Sdk.Models.Fulfillment;

/// <summary>Response envelope for <c>/order/package/document/get</c>.</summary>
public sealed record PrintAwbResponse
{
    [JsonPropertyName("code")]       public string? Code { get; init; }
    [JsonPropertyName("type")]       public string? Type { get; init; }
    [JsonPropertyName("message")]    public string? Message { get; init; }
    [JsonPropertyName("request_id")] public string? RequestId { get; init; }
    [JsonPropertyName("result")]     public PrintAwbResult? Result { get; init; }
}

public sealed record PrintAwbResult
{
    [JsonPropertyName("success")]
    [JsonConverter(typeof(StringOrBoolJsonConverter))]
    public bool Success { get; init; }

    [JsonPropertyName("error_code")] public string? ErrorCode { get; init; }
    [JsonPropertyName("error_msg")]  public string? ErrorMsg { get; init; }
    [JsonPropertyName("data")]       public PrintAwbData? Data { get; init; }
}

public sealed record PrintAwbData
{
    /// <summary>Base64-encoded document content. Use <see cref="GetFileBytes"/> to decode.</summary>
    [JsonPropertyName("file")]     public string? File { get; init; }

    /// <summary>Optional public URL to the PDF (when Lazada hosts it directly).</summary>
    [JsonPropertyName("pdf_url")]  public string? PdfUrl { get; init; }

    /// <summary>Document type echoed back (e.g. <c>"PDF"</c>).</summary>
    [JsonPropertyName("doc_type")] public string? DocType { get; init; }

    /// <summary>Decode <see cref="File"/> into raw bytes. Empty array when missing.</summary>
    public byte[] GetFileBytes()
        => string.IsNullOrEmpty(File) ? [] : Convert.FromBase64String(File);
}
