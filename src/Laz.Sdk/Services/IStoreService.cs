using Laz.Sdk.Models.Store;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the Store Decoration API endpoints (<c>/store/*</c>).
/// Access via <see cref="ILazClient.Store"/>.
/// </summary>
public interface IStoreService
{
    /// <summary>
    /// Get store custom pages. Calls <c>/store/custom/page/get</c>.
    /// </summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="size">Page size.</param>
    /// <param name="keyword">Optional keyword filter.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated store custom pages.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetStoreCustomPageResponse> GetStoreCustomPageAsync(
        int page,
        int size,
        string? keyword,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
