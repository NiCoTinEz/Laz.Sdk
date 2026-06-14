using Laz.Sdk.Models.Seller;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the Seller API endpoints (<c>/seller/*</c>, <c>/shop/*</c>, <c>/rc/*</c>, <c>/warehouse/*</c>).
/// Access via <c>ILazClient.Seller</c>.
/// </summary>
public interface ISellerService
{
    /// <summary>Get seller information. Calls <c>/seller/get</c>.</summary>
    Task<GetSellerResponse> GetSellerAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get seller metrics. Calls <c>/seller/metrics/get</c>.</summary>
    Task<GetSellerMetricsResponse> GetSellerMetricsAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get seller performance indicators. Calls <c>/seller/performance/get</c>.</summary>
    Task<GetSellerPerformanceResponse> GetSellerPerformanceAsync(
        string accessToken,
        string? language = null,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Batch query follow status for buyers. Calls <c>/shop/follow/status/batch/query</c>.</summary>
    Task<BatchQueryFollowStatusResponse> BatchQueryFollowStatusAsync(
        string[] buyerIds,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get pickup store list. Calls <c>/rc/store/list/get</c>.</summary>
    Task<GetPickUpStoreListResponse> GetPickUpStoreListAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get warehouse list by seller id. Calls <c>/warehouse/seller/get</c>.</summary>
    Task<GetWarehouseBySellerIdResponse> GetWarehouseBySellerIdAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Query warehouse detail. Calls <c>/warehouse/detail/query</c>.</summary>
    Task<QueryWarehouseDetailResponse> QueryWarehouseDetailAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Fetch seller policy. Calls <c>/seller/policy/fetch</c>.</summary>
    Task<SellerPolicyFetchResponse> SellerPolicyFetchAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get seller register info. Calls <c>/seller/register/info/get</c>.</summary>
    Task<GetSellerRegisterInfoResponse> GetSellerRegisterInfoAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get sub-address. Calls <c>/seller/address/sub/get</c>.</summary>
    Task<GetSubAddressResponse> GetSubAddressAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Bind payment method. Calls <c>/seller/payment/binding</c>.</summary>
    Task<PaymentBindingResponse> PaymentBindingAsync(
        PaymentBindingRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Verify a seller field. Calls <c>/seller/field/verify</c>.</summary>
    Task<SellerFieldVerifyResponse> SellerFieldVerifyAsync(
        SellerFieldVerifyRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get country info. Calls <c>/seller/country/info/get</c>.</summary>
    Task<GetCountryInfoResponse> GetCountryInfoAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
