using System.Text.Json;

namespace Laz.Sdk;

/// <summary>
/// Lazada Open Platform response envelope.
/// </summary>
public sealed class LazResponse
{
    /// <summary>Error type for error responses.</summary>
    public string? Type { get; init; }

    /// <summary>Error code. Zero (or absent) indicates success.</summary>
    public string? Code { get; init; }

    /// <summary>Human-readable error message.</summary>
    public string? Message { get; init; }

    /// <summary>Server-assigned request id.</summary>
    public string? RequestId { get; init; }

    /// <summary>Raw JSON response body.</summary>
    public string? Body { get; init; }

    /// <summary>True when <see cref="Code"/> is present and non-zero.</summary>
    public bool IsError() => !string.IsNullOrEmpty(Code) && !string.Equals(Code, "0", StringComparison.Ordinal);

    /// <summary>
    /// Deserialize <see cref="Body"/> into <typeparamref name="T"/> via <see cref="JsonSerializer"/>.
    /// Returns <c>default</c> when the body is empty.
    /// </summary>
    public T? ReadAs<T>(JsonSerializerOptions? options = null)
        => string.IsNullOrEmpty(Body) ? default : JsonSerializer.Deserialize<T>(Body, options);
}
