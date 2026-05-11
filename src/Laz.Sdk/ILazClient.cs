using Laz.Sdk.Models;

namespace Laz.Sdk;

/// <summary>
/// Async client for the Lazada Open Platform REST API.
/// </summary>
public interface ILazClient
{
    /// <summary>
    /// Execute a signed request against the Lazada Open Platform gateway configured
    /// via <see cref="LazClientOptions.ServerUrl"/>.
    /// </summary>
    /// <param name="request">The API request.</param>
    /// <param name="accessToken">Optional access token (omit for endpoints that do not require one).</param>
    /// <param name="timestamp">Optional request timestamp. Defaults to <see cref="DateTimeOffset.UtcNow"/>; caller values are coerced to UTC.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<LazResponse> ExecuteAsync(
        LazRequest request,
        string? accessToken = null,
        DateTime? timestamp = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Exchange an OAuth authorization <paramref name="code"/> for an access token.
    /// Calls <c>/auth/token/create</c> on the Lazada auth gateway
    /// (<see cref="Util.UrlConstants.API_AUTHORIZATION_URL"/>).
    /// </summary>
    /// <param name="code">Authorization code received from the OAuth redirect.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Token payload including access + refresh tokens and seller info.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<LazAccessToken> CreateAccessTokenAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh an access token using a previously issued <paramref name="refreshToken"/>.
    /// Calls <c>/auth/token/refresh</c> on the Lazada auth gateway.
    /// </summary>
    /// <param name="refreshToken">Refresh token from a prior token exchange.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Token payload with a new access token (and possibly a new refresh token).</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<LazAccessToken> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
