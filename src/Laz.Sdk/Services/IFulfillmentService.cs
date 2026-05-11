using Laz.Sdk.Models.Fulfillment;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps Lazada's <c>Fulfillment API</c> family (newer Pack/RTS, AWB printing, repack,
/// shipment-provider lookup, DBS/SOF delivery confirmations). Access via
/// <see cref="ILazClient.Fulfillment"/>.
/// </summary>
public interface IFulfillmentService
{
    /// <summary>
    /// List active shipment providers for one or more orders. Required input for the
    /// <c>shipping_provider</c> argument on the Pack and Ready-To-Ship endpoints.
    /// Calls <c>/order/shipment/providers/get</c>.
    /// </summary>
    Task<GetShipmentProvidersResponse> GetShipmentProvidersAsync(
        GetShipmentProvidersRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
