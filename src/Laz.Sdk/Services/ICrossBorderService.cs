using Laz.Sdk.Models.CrossBorder;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the Cross Border Product API endpoints (<c>/product/global/*</c>).
/// Access via <see cref="ILazClient.CrossBorder"/>.
/// </summary>
public interface ICrossBorderService
{
    /// <summary>
    /// Create a global product. Calls <c>/product/global/create</c>.
    /// </summary>
    /// <param name="payload">XML string describing the global product.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Creation result with SKU list.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<CreateGlobalProductResponse> CreateGlobalProductAsync(
        string payload,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get global product extension info. Calls <c>/product/global/extension</c>.
    /// </summary>
    /// <param name="globalItemIds">Optional list of global item IDs.</param>
    /// <param name="itemIds">Optional list of item IDs.</param>
    /// <param name="country">Country code.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Extension info array.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetGlobalProductExtensionResponse> GetGlobalProductExtensionAsync(
        IReadOnlyList<long>? globalItemIds,
        IReadOnlyList<long>? itemIds,
        string country,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update SKU for a global product. Calls <c>/product/global/sku/update</c>.
    /// </summary>
    /// <param name="payload">XML string with SKU update data.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Update result with SKU list.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<UpdateGlobalSkuResponse> UpdateGlobalSkuAsync(
        string payload,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if seller is cross-border enabled. Calls <c>/product/global/seller/status</c>.
    /// </summary>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Seller cross-border status.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetGlobalSellerStatusResponse> GetGlobalSellerStatusAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
