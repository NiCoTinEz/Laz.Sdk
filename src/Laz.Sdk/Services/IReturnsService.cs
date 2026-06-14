using Laz.Sdk.Models.Returns;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the Lazada Return &amp; Refund API endpoints.
/// Access via <see cref="ILazClient.Returns"/>.
/// </summary>
public interface IReturnsService
{
    /// <summary>
    /// Get detailed reverse order information. Calls <c>/order/reverse/return/detail/list</c>.
    /// </summary>
    /// <param name="request">Reverse order id.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Detailed reverse order info.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetReverseOrderDetailResponse> GetReverseOrderDetailAsync(
        GetReverseOrderDetailRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get history list for a reverse order line. Calls <c>/order/reverse/return/history/list</c>.
    /// </summary>
    /// <param name="request">Reverse order line id with optional paging.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated history list.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetReverseOrderHistoryListResponse> GetReverseOrderHistoryListAsync(
        GetReverseOrderHistoryListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get available reason list for a reverse order line. Calls <c>/order/reverse/reason/list</c>.
    /// </summary>
    /// <param name="request">Reverse order line id.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of reason objects.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetReverseOrderReasonListResponse> GetReverseOrderReasonListAsync(
        GetReverseOrderReasonListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get reverse orders for seller with filtering and paging. Calls <c>/reverse/getreverseordersforseller</c>.
    /// </summary>
    /// <param name="request">Filter and paging parameters.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of reverse orders.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetReverseOrdersForSellerResponse> GetReverseOrdersForSellerAsync(
        GetReverseOrdersForSellerRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validate whether an order can be cancelled. Calls <c>/order/reverse/cancel/validate</c>.
    /// </summary>
    /// <param name="request">Order/item identifiers.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<ReverseOrderCancelValidateResponse> ReverseOrderCancelValidateAsync(
        ReverseOrderCancelValidateRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiate a reverse order cancellation. Calls <c>/order/reverse/cancel/create</c>.
    /// </summary>
    /// <param name="request">Order id and cancel reason.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cancellation result.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<InitReverseOrderCancelResponse> InitReverseOrderCancelAsync(
        InitReverseOrderCancelRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a reverse order return (accept/refund/reject). Calls <c>/order/reverse/return/update</c>.
    /// </summary>
    /// <param name="request">Reverse order line id, action, and optional reason.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Update result.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<ReverseOrderReturnUpdateResponse> ReverseOrderReturnUpdateAsync(
        ReverseOrderReturnUpdateRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
