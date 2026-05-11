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

    /// <summary>
    /// Print AWB (Air Waybill / shipping label) for one or more packages.
    /// Calls <c>/order/package/document/get</c>. The response contains a Base64-encoded file
    /// (and optionally a hosted PDF URL).
    /// </summary>
    Task<PrintAwbResponse> PrintAwbAsync(
        PrintAwbRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Pack v2 — newer fulfillment-grouped pack endpoint that supports batch + multi-order.
    /// Calls <c>/order/fulfill/pack</c>. Coexists with the legacy
    /// <see cref="IOrdersService.PackAsync"/> on <c>/order/pack</c>.
    /// </summary>
    Task<PackV2Response> PackV2Async(
        PackV2Request request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
