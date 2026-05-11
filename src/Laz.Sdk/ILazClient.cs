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

    /// <summary>Fulfillment endpoints (Pack v2, RTS v2, PrintAWB, repack, DBS/SOF delivery).</summary>
    IFulfillmentService Fulfillment { get; }

    /// <summary>Logistics endpoints (tracking, 3PL station / runsheet management, scan, LDP).</summary>
    ILogisticsService Logistics { get; }

    /// <summary>
    /// Execute a signed request against the Lazada Open Platform gateway configured
    /// via <see cref="LazClientOptions.ServerUrl"/>. Prefer the typed wrappers under
    /// <see cref="Auth"/>, <see cref="Orders"/>, etc.
    /// </summary>
    /// <param name="request">The API request.</param>
    /// <param name="accessToken">Optional access token (omit for endpoints that do not require one).</param>
    /// <param name="timestamp">Optional request timestamp. Defaults to <see cref="DateTimeOffset.UtcNow"/>; caller values are coerced to UTC.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="credentials">
    /// Optional per-call credential override. Takes precedence over both <see cref="WithCredentials"/>
    /// and <see cref="LazClientOptions.AppKey"/> / <see cref="LazClientOptions.AppSecret"/>.
    /// Use when you want creds inline on each call instead of via a scoped client.
    /// </param>
    Task<LazResponse> ExecuteAsync(
        LazRequest request,
        string? accessToken = null,
        DateTime? timestamp = null,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Return a new <see cref="ILazClient"/> bound to the supplied <paramref name="credentials"/>.
    /// The returned client shares the underlying <see cref="HttpClient"/> and other options but
    /// signs every request with the override app key / app secret (and optional regional URL).
    /// Original client is unaffected. Cheap to call; safe to use per request.
    /// </summary>
    /// <example>
    /// <code>
    /// var tenant = client.WithCredentials(new LazCredentials(tenantKey, tenantSecret));
    /// var doc    = await tenant.Orders.GetDocumentAsync(req, tenantAccessToken, ct);
    /// </code>
    /// </example>
    ILazClient WithCredentials(LazCredentials credentials);
}
