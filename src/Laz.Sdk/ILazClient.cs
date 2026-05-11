using Laz.Sdk.Services;

namespace Laz.Sdk;

/// <summary>
/// Async client for the Lazada Open Platform REST API. Public endpoints are exposed via
/// service-grouped properties (<see cref="Auth"/>, <see cref="Orders"/>, ...).
/// Use <see cref="ExecuteAsync"/> directly only when an endpoint is not yet wrapped.
/// </summary>
public interface ILazClient
{
    /// <summary>Auth-gateway endpoints (token create / refresh).</summary>
    IAuthService Auth { get; }

    /// <summary>Order-related endpoints (<c>/order/*</c>, <c>/orders/*</c>).</summary>
    IOrdersService Orders { get; }

    /// <summary>
    /// Execute a signed request against the Lazada Open Platform gateway configured
    /// via <see cref="LazClientOptions.ServerUrl"/>. Prefer the typed wrappers under
    /// <see cref="Auth"/>, <see cref="Orders"/>, etc.
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
}
