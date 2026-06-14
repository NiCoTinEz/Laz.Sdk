using System.Globalization;
using Laz.Sdk.Models.Returns;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class ReturnsService(ILazClient client) : IReturnsService
{
    private readonly ILazClient _client = client;

    public async Task<GetReverseOrderDetailResponse> GetReverseOrderDetailAsync(
        GetReverseOrderDetailRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/order/reverse/return/detail/list") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("reverse_order_id", request.ReverseOrderId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetReverseOrderDetailResponse>();
    }

    public async Task<GetReverseOrderHistoryListResponse> GetReverseOrderHistoryListAsync(
        GetReverseOrderHistoryListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/order/reverse/return/history/list") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("reverse_order_line_id", request.ReverseOrderLineId.ToString(CultureInfo.InvariantCulture));
        if (request.PageSize is { } ps)
            lazRequest.AddApiParameter("page_size", ps.ToString(CultureInfo.InvariantCulture));
        if (request.PageNumber is { } pn)
            lazRequest.AddApiParameter("page_number", pn.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetReverseOrderHistoryListResponse>();
    }

    public async Task<GetReverseOrderReasonListResponse> GetReverseOrderReasonListAsync(
        GetReverseOrderReasonListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/order/reverse/reason/list") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("reverse_order_line_id", request.ReverseOrderLineId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetReverseOrderReasonListResponse>();
    }

    public async Task<GetReverseOrdersForSellerResponse> GetReverseOrdersForSellerAsync(
        GetReverseOrdersForSellerRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/reverse/getreverseordersforseller") { HttpMethod = Constants.METHOD_GET };
        if (!string.IsNullOrEmpty(request.RequestTypeList))
            lazRequest.AddApiParameter("request_type_list", request.RequestTypeList);
        if (!string.IsNullOrEmpty(request.OfcStatusList))
            lazRequest.AddApiParameter("ofc_status_list", request.OfcStatusList);
        if (request.Offset is { } off)
            lazRequest.AddApiParameter("offset", off.ToString(CultureInfo.InvariantCulture));
        if (request.PageSize is { } ps)
            lazRequest.AddApiParameter("page_size", ps.ToString(CultureInfo.InvariantCulture));
        if (request.CreatedAfter is { } ca)
            lazRequest.AddApiParameter("created_after", ca.ToString(CultureInfo.InvariantCulture));
        if (request.CreatedBefore is { } cb)
            lazRequest.AddApiParameter("created_before", cb.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetReverseOrdersForSellerResponse>();
    }

    public async Task<ReverseOrderCancelValidateResponse> ReverseOrderCancelValidateAsync(
        ReverseOrderCancelValidateRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/order/reverse/cancel/validate") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("order_id", request.OrderId.ToString(CultureInfo.InvariantCulture));
        if (request.OrderItemId is { } oi)
            lazRequest.AddApiParameter("order_item_id", oi.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<ReverseOrderCancelValidateResponse>();
    }

    public async Task<InitReverseOrderCancelResponse> InitReverseOrderCancelAsync(
        InitReverseOrderCancelRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.Reason);

        var lazRequest = new LazRequest("/order/reverse/cancel/create");
        lazRequest.AddApiParameter("order_id", request.OrderId.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("reason", request.Reason);
        if (request.OrderItemId is { } oi)
            lazRequest.AddApiParameter("order_item_id", oi.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<InitReverseOrderCancelResponse>();
    }

    public async Task<ReverseOrderReturnUpdateResponse> ReverseOrderReturnUpdateAsync(
        ReverseOrderReturnUpdateRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.Action);

        var lazRequest = new LazRequest("/order/reverse/return/update");
        lazRequest.AddApiParameter("reverse_order_line_id", request.ReverseOrderLineId.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("action", request.Action);
        if (!string.IsNullOrEmpty(request.Reason))
            lazRequest.AddApiParameter("reason", request.Reason);
        if (!string.IsNullOrEmpty(request.ReasonDetail))
            lazRequest.AddApiParameter("reason_detail", request.ReasonDetail);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<ReverseOrderReturnUpdateResponse>();
    }
}
