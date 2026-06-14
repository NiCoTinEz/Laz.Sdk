using System.Globalization;
using Laz.Sdk.Models.Seller;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class SellerService(ILazClient client) : ISellerService
{
    private readonly ILazClient _client = client;

    public async Task<GetSellerResponse> GetSellerAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetSellerResponse>();
    }

    public async Task<GetSellerMetricsResponse> GetSellerMetricsAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/metrics/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetSellerMetricsResponse>();
    }

    public async Task<GetSellerPerformanceResponse> GetSellerPerformanceAsync(
        string accessToken,
        string? language = null,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/performance/get") { HttpMethod = Constants.METHOD_GET };
        if (!string.IsNullOrEmpty(language))
            lazRequest.AddApiParameter("language", language);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetSellerPerformanceResponse>();
    }

    public async Task<BatchQueryFollowStatusResponse> BatchQueryFollowStatusAsync(
        string[] buyerIds,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(buyerIds);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/shop/follow/status/batch/query") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("buyer_ids", string.Join(",", buyerIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<BatchQueryFollowStatusResponse>();
    }

    public async Task<GetPickUpStoreListResponse> GetPickUpStoreListAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/rc/store/list/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetPickUpStoreListResponse>();
    }

    public async Task<GetWarehouseBySellerIdResponse> GetWarehouseBySellerIdAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/warehouse/seller/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetWarehouseBySellerIdResponse>();
    }

    public async Task<QueryWarehouseDetailResponse> QueryWarehouseDetailAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/warehouse/detail/query") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<QueryWarehouseDetailResponse>();
    }

    public async Task<SellerPolicyFetchResponse> SellerPolicyFetchAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/policy/fetch") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<SellerPolicyFetchResponse>();
    }

    public async Task<GetSellerRegisterInfoResponse> GetSellerRegisterInfoAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/register/info/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetSellerRegisterInfoResponse>();
    }

    public async Task<GetSubAddressResponse> GetSubAddressAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/address/sub/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetSubAddressResponse>();
    }

    public async Task<PaymentBindingResponse> PaymentBindingAsync(
        PaymentBindingRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/payment/binding");
        if (!string.IsNullOrEmpty(request.PaymentMethod))
            lazRequest.AddApiParameter("payment_method", request.PaymentMethod);
        if (!string.IsNullOrEmpty(request.AccountNo))
            lazRequest.AddApiParameter("account_no", request.AccountNo);
        if (!string.IsNullOrEmpty(request.AccountName))
            lazRequest.AddApiParameter("account_name", request.AccountName);
        if (!string.IsNullOrEmpty(request.BankCode))
            lazRequest.AddApiParameter("bank_code", request.BankCode);
        if (!string.IsNullOrEmpty(request.BranchCode))
            lazRequest.AddApiParameter("branch_code", request.BranchCode);
        if (!string.IsNullOrEmpty(request.PaymentDetails))
            lazRequest.AddApiParameter("payment_details", request.PaymentDetails);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<PaymentBindingResponse>();
    }

    public async Task<SellerFieldVerifyResponse> SellerFieldVerifyAsync(
        SellerFieldVerifyRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/field/verify");
        if (!string.IsNullOrEmpty(request.FieldName))
            lazRequest.AddApiParameter("field_name", request.FieldName);
        if (!string.IsNullOrEmpty(request.FieldValue))
            lazRequest.AddApiParameter("field_value", request.FieldValue);
        if (!string.IsNullOrEmpty(request.ExtraData))
            lazRequest.AddApiParameter("extra_data", request.ExtraData);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<SellerFieldVerifyResponse>();
    }

    public async Task<GetCountryInfoResponse> GetCountryInfoAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/seller/country/info/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetCountryInfoResponse>();
    }
}
