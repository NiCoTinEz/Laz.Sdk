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
}
