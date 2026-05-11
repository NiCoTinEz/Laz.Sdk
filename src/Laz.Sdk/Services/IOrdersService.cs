using Laz.Sdk.Models.Orders;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the <c>/orders/*</c> family of Lazada Open Platform endpoints.
/// Access via <see cref="ILazClient.Orders"/>.
/// </summary>
public interface IOrdersService
{
    /// <summary>
    /// Retrieve an order document (invoice, shipping label, or carrier manifest) for the
    /// specified order items. Calls <c>/order/document/get</c>.
    /// </summary>
    /// <param name="request">Document type + order item ids.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override (takes precedence over scoped + options).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Document payload with Base64-encoded content.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetOrderDocumentResponse> GetDocumentAsync(
        GetOrderDocumentRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// List orders matching the given filters. Calls <c>/orders/get</c>.
    /// At least one of <see cref="GetOrdersRequest.UpdateAfter"/> /
    /// <see cref="GetOrdersRequest.CreatedAfter"/> is required by Lazada.
    /// </summary>
    /// <param name="request">Filter + paging + sort options.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Page of orders with <c>count</c> and <c>countTotal</c>.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetOrdersResponse> GetOrdersAsync(
        GetOrdersRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single order by id. Calls <c>/order/get</c>.
    /// </summary>
    /// <param name="request">Order id.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Single order envelope.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetOrderResponse> GetOrderAsync(
        GetOrderRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the line items of a single order. Calls <c>/order/items/get</c>.
    /// </summary>
    Task<GetOrderItemsResponse> GetOrderItemsAsync(
        GetOrderItemsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get line items for up to 50 orders in one call. Calls <c>/orders/items/get</c>.
    /// </summary>
    Task<GetMultipleOrderItemsResponse> GetMultipleOrderItemsAsync(
        GetMultipleOrderItemsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Mark order items as packed. Calls <c>/order/pack</c>. All ids must belong to the same order.
    /// </summary>
    Task<PackOrderResponse> PackAsync(
        PackOrderRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Mark order items as ready-to-ship. Calls <c>/order/rts</c>. All ids must belong to the same order.
    /// </summary>
    Task<ReadyToShipResponse> ReadyToShipAsync(
        ReadyToShipRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
