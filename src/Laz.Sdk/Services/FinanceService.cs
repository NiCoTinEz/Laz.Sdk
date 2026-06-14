using System.Globalization;
using Laz.Sdk.Models.Finance;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class FinanceService(ILazClient client) : IFinanceService
{
    private readonly ILazClient _client = client;

    public async Task<GetPayoutStatusResponse> GetPayoutStatusAsync(
        GetPayoutStatusRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.CreatedAfter);

        var lazRequest = new LazRequest("/finance/payout/status/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("created_after", request.CreatedAfter);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetPayoutStatusResponse>();
    }

    public async Task<QueryAccountTransactionsResponse> QueryAccountTransactionsAsync(
        QueryAccountTransactionsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.StartTime);
        ArgumentException.ThrowIfNullOrEmpty(request.EndTime);
        if (request.PageSize <= 0)
        {
            throw new ArgumentException("PageSize must be greater than zero.", nameof(request));
        }
        if (request.PageNum <= 0)
        {
            throw new ArgumentException("PageNum must be greater than zero.", nameof(request));
        }

        var lazRequest = new LazRequest("/finance/transaction/accountTransactions/query");
        if (!string.IsNullOrEmpty(request.TransactionType))
        {
            lazRequest.AddApiParameter("transaction_type", request.TransactionType);
        }
        if (!string.IsNullOrEmpty(request.SubTransactionType))
        {
            lazRequest.AddApiParameter("sub_transaction_type", request.SubTransactionType);
        }
        if (!string.IsNullOrEmpty(request.TransactionNumber))
        {
            lazRequest.AddApiParameter("transaction_number", request.TransactionNumber);
        }
        lazRequest.AddApiParameter("page_size", request.PageSize.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("start_time", request.StartTime);
        lazRequest.AddApiParameter("end_time", request.EndTime);
        lazRequest.AddApiParameter("page_num", request.PageNum.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<QueryAccountTransactionsResponse>();
    }

    public async Task<QueryLogisticsFeeDetailResponse> QueryLogisticsFeeDetailAsync(
        QueryLogisticsFeeDetailRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/lbs/slb/queryLogisticsFeeDetail") { HttpMethod = Constants.METHOD_GET };
        if (!string.IsNullOrEmpty(request.SellerId))
        {
            lazRequest.AddApiParameter("seller_id", request.SellerId);
        }
        if (!string.IsNullOrEmpty(request.RequestType))
        {
            lazRequest.AddApiParameter("request_type", request.RequestType);
        }
        if (!string.IsNullOrEmpty(request.StartTime))
        {
            lazRequest.AddApiParameter("start_time", request.StartTime);
        }
        if (!string.IsNullOrEmpty(request.EndTime))
        {
            lazRequest.AddApiParameter("end_time", request.EndTime);
        }
        if (request.PageNum is { } pn)
        {
            lazRequest.AddApiParameter("page_num", pn.ToString(CultureInfo.InvariantCulture));
        }
        if (request.PageSize is { } ps)
        {
            lazRequest.AddApiParameter("page_size", ps.ToString(CultureInfo.InvariantCulture));
        }

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<QueryLogisticsFeeDetailResponse>();
    }

    public async Task<QueryTransactionDetailsResponse> QueryTransactionDetailsAsync(
        QueryTransactionDetailsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.StartDate);
        ArgumentException.ThrowIfNullOrEmpty(request.EndDate);

        var lazRequest = new LazRequest("/finance/seller/transaction/detail") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("start_date", request.StartDate);
        lazRequest.AddApiParameter("end_date", request.EndDate);
        if (request.PageNum is { } pn)
        {
            lazRequest.AddApiParameter("page_num", pn.ToString(CultureInfo.InvariantCulture));
        }
        if (request.PageSize is { } ps)
        {
            lazRequest.AddApiParameter("page_size", ps.ToString(CultureInfo.InvariantCulture));
        }

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<QueryTransactionDetailsResponse>();
    }
}
