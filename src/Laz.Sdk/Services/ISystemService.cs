using Laz.Sdk.Models.System;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps System API endpoints (<c>/auth/token/createWithOpenId</c>, <c>/data/mop/format/get</c>).
/// Access via <see cref="ILazClient.System"/>.
/// </summary>
public interface ISystemService
{
    /// <summary>
    /// Create an access token with OpenId. Calls <c>/auth/token/createWithOpenId</c>.
    /// </summary>
    /// <param name="code">Authorization code from the OAuth redirect.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Access token with OpenId.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<LazAccessTokenWithOpenId> CreateAccessTokenWithOpenIdAsync(
        string code,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get data format for bulk operations. Calls <c>/data/mop/format/get</c>.
    /// </summary>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Data format info.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetDataMopFormatResponse> GetDataMopFormatAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
