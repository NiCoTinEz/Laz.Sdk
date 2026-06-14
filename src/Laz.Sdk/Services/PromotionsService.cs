using System.Globalization;
using System.Text;
using Laz.Sdk.Models.Promotions;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class PromotionsService(ILazClient client) : IPromotionsService
{
    private readonly ILazClient _client = client;

    // ──────────────────────────────────────────────
    // Seller Voucher
    // ──────────────────────────────────────────────

    public async Task<CreateVoucherResponse> CreateVoucherAsync(
        CreateVoucherRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/voucher/create");
        lazRequest.AddApiParameter("criteria_over_money", request.CriteriaOverMoney);
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("apply", request.Apply);
        lazRequest.AddApiParameter("collect_start", request.CollectStart);
        lazRequest.AddApiParameter("display_area", request.DisplayArea);
        lazRequest.AddApiParameter("period_end_time", request.PeriodEndTime);
        lazRequest.AddApiParameter("voucher_name", request.VoucherName);
        lazRequest.AddApiParameter("voucher_discount_type", request.VoucherDiscountType);
        lazRequest.AddApiParameter("offering_money_value_off", request.OfferingMoneyValueOff);
        lazRequest.AddApiParameter("period_start_time", request.PeriodStartTime);
        lazRequest.AddApiParameter("limit", request.Limit);
        lazRequest.AddApiParameter("issued", request.Issued);
        lazRequest.AddApiParameter("max_discount_offering_money_value", request.MaxDiscountOfferingMoneyValue);
        lazRequest.AddApiParameter("offering_percentage_discount_off", request.OfferingPercentageDiscountOff);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<CreateVoucherResponse>();
    }

    public async Task<UpdateVoucherResponse> UpdateVoucherAsync(
        UpdateVoucherRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/voucher/update");
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("criteria_over_money", request.CriteriaOverMoney);
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("apply", request.Apply);
        lazRequest.AddApiParameter("collect_start", request.CollectStart);
        lazRequest.AddApiParameter("display_area", request.DisplayArea);
        lazRequest.AddApiParameter("period_end_time", request.PeriodEndTime);
        lazRequest.AddApiParameter("voucher_name", request.VoucherName);
        lazRequest.AddApiParameter("voucher_discount_type", request.VoucherDiscountType);
        lazRequest.AddApiParameter("offering_money_value_off", request.OfferingMoneyValueOff);
        lazRequest.AddApiParameter("period_start_time", request.PeriodStartTime);
        lazRequest.AddApiParameter("limit", request.Limit);
        lazRequest.AddApiParameter("issued", request.Issued);
        lazRequest.AddApiParameter("max_discount_offering_money_value", request.MaxDiscountOfferingMoneyValue);
        lazRequest.AddApiParameter("offering_percentage_discount_off", request.OfferingPercentageDiscountOff);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<UpdateVoucherResponse>();
    }

    public async Task<GetVoucherResponse> GetVoucherAsync(
        GetVoucherRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/voucher/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetVoucherResponse>();
    }

    public async Task<GetVoucherListResponse> GetVoucherListAsync(
        GetVoucherListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/vouchers/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("offset", request.Offset);
        lazRequest.AddApiParameter("limit", request.Limit);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetVoucherListResponse>();
    }

    public async Task<VoucherActionResponse> ActivateVoucherAsync(
        VoucherActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/voucher/activate");
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<VoucherActionResponse>();
    }

    public async Task<VoucherActionResponse> DeactivateVoucherAsync(
        VoucherActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/voucher/deactivate");
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<VoucherActionResponse>();
    }

    public async Task<GetVoucherProductsResponse> GetVoucherProductsAsync(
        GetVoucherProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/voucher/products/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("offset", request.Offset);
        lazRequest.AddApiParameter("limit", request.Limit);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetVoucherProductsResponse>();
    }

    public async Task<AddVoucherSkuResponse> AddVoucherSkuAsync(
        VoucherSkuRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/voucher/product/sku/add");
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("sku_ids", FormatIds(request.SkuIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<AddVoucherSkuResponse>();
    }

    public async Task<RemoveVoucherSkuResponse> RemoveVoucherSkuAsync(
        VoucherSkuRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/voucher/product/sku/remove");
        lazRequest.AddApiParameter("voucher_type", request.VoucherType);
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("sku_ids", FormatIds(request.SkuIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<RemoveVoucherSkuResponse>();
    }

    // ──────────────────────────────────────────────
    // Free Shipping
    // ──────────────────────────────────────────────

    public async Task<CreateFreeShippingResponse> CreateFreeShippingAsync(
        CreateFreeShippingRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshipping/create");
        lazRequest.AddApiParameter("free_shipping_name", request.FreeShippingName);
        lazRequest.AddApiParameter("free_shipping_type", request.FreeShippingType);
        lazRequest.AddApiParameter("start_time", request.StartTime);
        lazRequest.AddApiParameter("end_time", request.EndTime);
        lazRequest.AddApiParameter("apply", request.Apply);
        lazRequest.AddApiParameter("criteria_over_money", request.CriteriaOverMoney);
        lazRequest.AddApiParameter("display_area", request.DisplayArea);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<CreateFreeShippingResponse>();
    }

    public async Task<UpdateFreeShippingResponse> UpdateFreeShippingAsync(
        UpdateFreeShippingRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshipping/update");
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("free_shipping_name", request.FreeShippingName);
        lazRequest.AddApiParameter("free_shipping_type", request.FreeShippingType);
        lazRequest.AddApiParameter("start_time", request.StartTime);
        lazRequest.AddApiParameter("end_time", request.EndTime);
        lazRequest.AddApiParameter("apply", request.Apply);
        lazRequest.AddApiParameter("criteria_over_money", request.CriteriaOverMoney);
        lazRequest.AddApiParameter("display_area", request.DisplayArea);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<UpdateFreeShippingResponse>();
    }

    public async Task<GetFreeShippingResponse> GetFreeShippingAsync(
        GetFreeShippingRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshipping/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetFreeShippingResponse>();
    }

    public async Task<GetFreeShippingListResponse> GetFreeShippingListAsync(
        GetFreeShippingListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshippings/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("offset", request.Offset);
        lazRequest.AddApiParameter("limit", request.Limit);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetFreeShippingListResponse>();
    }

    public async Task<FreeShippingActionResponse> ActivateFreeShippingAsync(
        FreeShippingActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshipping/activate");
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<FreeShippingActionResponse>();
    }

    public async Task<FreeShippingActionResponse> DeactivateFreeShippingAsync(
        FreeShippingActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshipping/deactivate");
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<FreeShippingActionResponse>();
    }

    public async Task<GetFreeShippingProductsResponse> GetFreeShippingProductsAsync(
        GetFreeShippingProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshipping/products/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("offset", request.Offset);
        lazRequest.AddApiParameter("limit", request.Limit);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetFreeShippingProductsResponse>();
    }

    public async Task<AddFreeShippingSkuResponse> AddFreeShippingSkuAsync(
        FreeShippingSkuRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshipping/product/sku/add");
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("sku_ids", FormatIds(request.SkuIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<AddFreeShippingSkuResponse>();
    }

    public async Task<RemoveFreeShippingSkuResponse> RemoveFreeShippingSkuAsync(
        FreeShippingSkuRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/freeshipping/product/sku/remove");
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("sku_ids", FormatIds(request.SkuIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<RemoveFreeShippingSkuResponse>();
    }

    // ──────────────────────────────────────────────
    // FlexiCombo
    // ──────────────────────────────────────────────

    public async Task<CreateFlexiComboResponse> CreateFlexiComboAsync(
        CreateFlexiComboRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/flexicombo/create");
        lazRequest.AddApiParameter("flexi_combo_name", request.FlexiComboName);
        lazRequest.AddApiParameter("flexi_combo_type", request.FlexiComboType);
        lazRequest.AddApiParameter("start_time", request.StartTime);
        lazRequest.AddApiParameter("end_time", request.EndTime);
        lazRequest.AddApiParameter("discount_type", request.DiscountType);
        lazRequest.AddApiParameter("discount_value", request.DiscountValue);
        lazRequest.AddApiParameter("min_spend", request.MinSpend);
        lazRequest.AddApiParameter("max_discount", request.MaxDiscount);
        lazRequest.AddApiParameter("apply", request.Apply);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<CreateFlexiComboResponse>();
    }

    public async Task<GetFlexiComboDetailsResponse> GetFlexiComboDetailsAsync(
        GetFlexiComboDetailsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/flexicombo/details") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetFlexiComboDetailsResponse>();
    }

    public async Task<UpdateFlexiComboResponse> UpdateFlexiComboAsync(
        UpdateFlexiComboRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/flexicombo/update");
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("flexi_combo_name", request.FlexiComboName);
        lazRequest.AddApiParameter("flexi_combo_type", request.FlexiComboType);
        lazRequest.AddApiParameter("start_time", request.StartTime);
        lazRequest.AddApiParameter("end_time", request.EndTime);
        lazRequest.AddApiParameter("discount_type", request.DiscountType);
        lazRequest.AddApiParameter("discount_value", request.DiscountValue);
        lazRequest.AddApiParameter("min_spend", request.MinSpend);
        lazRequest.AddApiParameter("max_discount", request.MaxDiscount);
        lazRequest.AddApiParameter("apply", request.Apply);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<UpdateFlexiComboResponse>();
    }

    public async Task<GetFlexiComboListResponse> GetFlexiComboListAsync(
        GetFlexiComboListRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/flexicombo/list") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("offset", request.Offset);
        lazRequest.AddApiParameter("limit", request.Limit);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetFlexiComboListResponse>();
    }

    public async Task<FlexiComboActionResponse> ActivateFlexiComboAsync(
        FlexiComboActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/flexicombo/activate");
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<FlexiComboActionResponse>();
    }

    public async Task<FlexiComboActionResponse> DeactivateFlexiComboAsync(
        FlexiComboActionRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/flexicombo/deactivate");
        lazRequest.AddApiParameter("id", request.Id);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<FlexiComboActionResponse>();
    }

    public async Task<AddFlexiComboProductsResponse> AddFlexiComboProductsAsync(
        FlexiComboProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/flexicombo/products/add");
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("product_ids", FormatIds(request.ProductIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<AddFlexiComboProductsResponse>();
    }

    public async Task<RemoveFlexiComboProductsResponse> RemoveFlexiComboProductsAsync(
        FlexiComboProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/promotion/flexicombo/products/remove");
        lazRequest.AddApiParameter("id", request.Id);
        lazRequest.AddApiParameter("product_ids", FormatIds(request.ProductIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<RemoveFlexiComboProductsResponse>();
    }

    // ──────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────

    private static string? FormatIds(IReadOnlyList<long>? ids)
    {
        if (ids is null || ids.Count == 0)
            return null;

        var sb = new StringBuilder(2 + ids.Count * 8);
        sb.Append('[');
        var first = true;
        foreach (var id in ids)
        {
            if (!first)
                sb.Append(',');
            sb.Append(id.ToString(CultureInfo.InvariantCulture));
            first = false;
        }
        sb.Append(']');
        return sb.ToString();
    }
}
