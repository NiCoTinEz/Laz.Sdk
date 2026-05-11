using Laz.Sdk.Models.Logistics;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps Lazada's <c>Logistics API</c> family (tracking, 3PL station / runsheet management,
/// scan + LDP last-mile updates). Access via <see cref="ILazClient.Logistics"/>.
/// </summary>
public interface ILogisticsService
{
    /// <summary>
    /// Get logistic trace (tracking events) for an order. Only available after ready-to-ship.
    /// Calls <c>/logistic/order/trace</c>.
    /// </summary>
    Task<GetOrderTraceResponse> GetOrderTraceAsync(
        GetOrderTraceRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
