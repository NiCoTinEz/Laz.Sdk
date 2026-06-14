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

    /// <summary>Finance endpoints (payout status, account transactions, logistics fees, transaction details).</summary>
    IFinanceService Finance { get; }

    /// <summary>Logistics endpoints (tracking, 3PL station / runsheet management, scan, LDP).</summary>
    ILogisticsService Logistics { get; }

    /// <summary>Product endpoints (<c>/product/*</c>, <c>/products/*</c>, <c>/category/*</c>, <c>/brands/*</c>, <c>/price/*</c>, <c>/images/*</c>, <c>/size/*</c>).</summary>
    IProductService Products { get; }

    /// <summary>Return &amp; Refund API endpoints (<c>/order/reverse/*</c>, <c>/reverse/*</c>).</summary>
    IReturnsService Returns { get; }

    /// <summary>Seller API endpoints (<c>/seller/*</c>, <c>/shop/*</c>, <c>/rc/*</c>, <c>/warehouse/*</c>).</summary>
    ISellerService Seller { get; }

    /// <summary>Promotions API endpoints (<c>/promotion/voucher/*</c>, <c>/promotion/freeshipping/*</c>, <c>/promotion/flexicombo/*</c>).</summary>
    IPromotionsService Promotions { get; }

    /// <summary>Media Center / Video API endpoints (<c>/media/video/*</c>, <c>/image/*</c>).</summary>
    IMediaService Media { get; }

    /// <summary>Product Review API endpoints (<c>/review/seller/*</c>).</summary>
    IReviewsService Reviews { get; }

    /// <summary>Cross Border Product API endpoints (<c>/product/global/*</c>).</summary>
    ICrossBorderService CrossBorder { get; }

    /// <summary>Store Decoration API endpoints (<c>/store/*</c>).</summary>
    IStoreService Store { get; }

    /// <summary>System API endpoints (<c>/auth/token/createWithOpenId</c>, <c>/data/mop/format/get</c>).</summary>
    ISystemService System { get; }

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
