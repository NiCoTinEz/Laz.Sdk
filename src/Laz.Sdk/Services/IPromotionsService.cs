using Laz.Sdk.Models.Promotions;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the Promotions API endpoints (<c>/promotion/voucher/*</c>, <c>/promotion/freeshipping/*</c>,
/// <c>/promotion/flexicombo/*</c>). Access via <c>ILazClient.Promotions</c>.
/// </summary>
public interface IPromotionsService
{
    // ──────────────────────────────────────────────
    // Seller Voucher (9 endpoints)
    // ──────────────────────────────────────────────

    /// <summary>Create a seller voucher. Calls <c>/promotion/voucher/create</c>.</summary>
    Task<CreateVoucherResponse> CreateVoucherAsync(
        CreateVoucherRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update a seller voucher. Calls <c>/promotion/voucher/update</c>.</summary>
    Task<UpdateVoucherResponse> UpdateVoucherAsync(
        UpdateVoucherRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get a seller voucher. Calls <c>/promotion/voucher/get</c>.</summary>
    Task<GetVoucherResponse> GetVoucherAsync(
        GetVoucherRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>List seller vouchers. Calls <c>/promotion/vouchers/get</c>.</summary>
    Task<GetVoucherListResponse> GetVoucherListAsync(
        GetVoucherListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Activate a seller voucher. Calls <c>/promotion/voucher/activate</c>.</summary>
    Task<VoucherActionResponse> ActivateVoucherAsync(
        VoucherActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Deactivate a seller voucher. Calls <c>/promotion/voucher/deactivate</c>.</summary>
    Task<VoucherActionResponse> DeactivateVoucherAsync(
        VoucherActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get products associated with a voucher. Calls <c>/promotion/voucher/products/get</c>.</summary>
    Task<GetVoucherProductsResponse> GetVoucherProductsAsync(
        GetVoucherProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Add SKUs to a voucher. Calls <c>/promotion/voucher/product/sku/add</c>.</summary>
    Task<AddVoucherSkuResponse> AddVoucherSkuAsync(
        VoucherSkuRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Remove SKUs from a voucher. Calls <c>/promotion/voucher/product/sku/remove</c>.</summary>
    Task<RemoveVoucherSkuResponse> RemoveVoucherSkuAsync(
        VoucherSkuRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    // ──────────────────────────────────────────────
    // Free Shipping (9 endpoints)
    // ──────────────────────────────────────────────

    /// <summary>Create a free shipping promotion. Calls <c>/promotion/freeshipping/create</c>.</summary>
    Task<CreateFreeShippingResponse> CreateFreeShippingAsync(
        CreateFreeShippingRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update a free shipping promotion. Calls <c>/promotion/freeshipping/update</c>.</summary>
    Task<UpdateFreeShippingResponse> UpdateFreeShippingAsync(
        UpdateFreeShippingRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get a free shipping promotion. Calls <c>/promotion/freeshipping/get</c>.</summary>
    Task<GetFreeShippingResponse> GetFreeShippingAsync(
        GetFreeShippingRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>List free shipping promotions. Calls <c>/promotion/freeshippings/get</c>.</summary>
    Task<GetFreeShippingListResponse> GetFreeShippingListAsync(
        GetFreeShippingListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Activate a free shipping promotion. Calls <c>/promotion/freeshipping/activate</c>.</summary>
    Task<FreeShippingActionResponse> ActivateFreeShippingAsync(
        FreeShippingActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Deactivate a free shipping promotion. Calls <c>/promotion/freeshipping/deactivate</c>.</summary>
    Task<FreeShippingActionResponse> DeactivateFreeShippingAsync(
        FreeShippingActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get products associated with a free shipping promotion. Calls <c>/promotion/freeshipping/products/get</c>.</summary>
    Task<GetFreeShippingProductsResponse> GetFreeShippingProductsAsync(
        GetFreeShippingProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Add SKUs to a free shipping promotion. Calls <c>/promotion/freeshipping/product/sku/add</c>.</summary>
    Task<AddFreeShippingSkuResponse> AddFreeShippingSkuAsync(
        FreeShippingSkuRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Remove SKUs from a free shipping promotion. Calls <c>/promotion/freeshipping/product/sku/remove</c>.</summary>
    Task<RemoveFreeShippingSkuResponse> RemoveFreeShippingSkuAsync(
        FreeShippingSkuRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    // ──────────────────────────────────────────────
    // FlexiCombo (8 endpoints)
    // ──────────────────────────────────────────────

    /// <summary>Create a FlexiCombo promotion. Calls <c>/promotion/flexicombo/create</c>.</summary>
    Task<CreateFlexiComboResponse> CreateFlexiComboAsync(
        CreateFlexiComboRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get FlexiCombo details. Calls <c>/promotion/flexicombo/details</c>.</summary>
    Task<GetFlexiComboDetailsResponse> GetFlexiComboDetailsAsync(
        GetFlexiComboDetailsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update a FlexiCombo promotion. Calls <c>/promotion/flexicombo/update</c>.</summary>
    Task<UpdateFlexiComboResponse> UpdateFlexiComboAsync(
        UpdateFlexiComboRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>List FlexiCombo promotions. Calls <c>/promotion/flexicombo/list</c>.</summary>
    Task<GetFlexiComboListResponse> GetFlexiComboListAsync(
        GetFlexiComboListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Activate a FlexiCombo promotion. Calls <c>/promotion/flexicombo/activate</c>.</summary>
    Task<FlexiComboActionResponse> ActivateFlexiComboAsync(
        FlexiComboActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Deactivate a FlexiCombo promotion. Calls <c>/promotion/flexicombo/deactivate</c>.</summary>
    Task<FlexiComboActionResponse> DeactivateFlexiComboAsync(
        FlexiComboActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Add products to a FlexiCombo promotion. Calls <c>/promotion/flexicombo/products/add</c>.</summary>
    Task<AddFlexiComboProductsResponse> AddFlexiComboProductsAsync(
        FlexiComboProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Remove products from a FlexiCombo promotion. Calls <c>/promotion/flexicombo/products/remove</c>.</summary>
    Task<RemoveFlexiComboProductsResponse> RemoveFlexiComboProductsAsync(
        FlexiComboProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
