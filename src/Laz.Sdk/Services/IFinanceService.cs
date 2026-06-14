using Laz.Sdk.Models.Finance;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the <c>/finance/*</c> family of Lazada Open Platform endpoints.
/// Access via <see cref="ILazClient.Finance"/>.
/// </summary>
public interface IFinanceService
{
    /// <summary>
    /// Get payout status for statements created after a given date. Calls <c>/finance/payout/status/get</c>.
    /// </summary>
    /// <param name="request">Filter with <see cref="GetPayoutStatusRequest.CreatedAfter"/>.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of payout statements.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<GetPayoutStatusResponse> GetPayoutStatusAsync(
        GetPayoutStatusRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Query account transactions with pagination. Calls <c>/finance/transaction/accountTransactions/query</c>.
    /// </summary>
    /// <param name="request">Transaction filters + paging.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated transaction list.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<QueryAccountTransactionsResponse> QueryAccountTransactionsAsync(
        QueryAccountTransactionsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Query logistics fee details. Calls <c>/lbs/slb/queryLogisticsFeeDetail</c>.
    /// </summary>
    /// <param name="request">Fee detail filters.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Logistics fee details.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<QueryLogisticsFeeDetailResponse> QueryLogisticsFeeDetailAsync(
        QueryLogisticsFeeDetailRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Query seller transaction details within a date range. Calls <c>/finance/seller/transaction/detail</c>.
    /// </summary>
    /// <param name="request">Date range + optional paging.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Transaction details.</returns>
    /// <exception cref="LazException">Thrown when the platform returns an error response.</exception>
    Task<QueryTransactionDetailsResponse> QueryTransactionDetailsAsync(
        QueryTransactionDetailsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
