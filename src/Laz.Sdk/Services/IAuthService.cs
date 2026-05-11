using Laz.Sdk.Models;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the <c>/auth/*</c> endpoints on the Lazada auth gateway
/// (<see cref="Util.UrlConstants.API_AUTHORIZATION_URL"/>).
/// Access via <see cref="ILazClient.Auth"/>.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Exchange an OAuth authorization <paramref name="code"/> for an access token via <c>/auth/token/create</c>.
    /// </summary>
    /// <param name="code">Authorization code received from the OAuth redirect.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Token payload with access + refresh tokens and seller info.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<LazAccessToken> CreateAccessTokenAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh an access token via <c>/auth/token/refresh</c>.
    /// </summary>
    /// <param name="refreshToken">Refresh token from a prior token exchange.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Token payload with a new access token (and possibly a new refresh token).</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<LazAccessToken> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
