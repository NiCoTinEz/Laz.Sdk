namespace Laz.Sdk;

/// <summary>
/// Async client for the Lazada Open Platform REST API.
/// </summary>
public interface ILazClient
{
    /// <summary>
    /// Execute a signed request against the Lazada Open Platform gateway.
    /// </summary>
    /// <param name="request">The API request.</param>
    /// <param name="accessToken">Optional access token (omit for endpoints that do not require it).</param>
    /// <param name="timestamp">Optional request timestamp. If <c>null</c>, <see cref="DateTimeOffset.UtcNow"/> is used. Caller-supplied values are coerced to UTC.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<LazResponse> ExecuteAsync(
        LazRequest request,
        string? accessToken = null,
        DateTime? timestamp = null,
        CancellationToken cancellationToken = default);
}
