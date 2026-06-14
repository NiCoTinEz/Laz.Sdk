using System.Globalization;
using Laz.Sdk.Models.Products;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class ProductService(ILazClient client) : IProductService
{
    private readonly ILazClient _client = client;

    public async Task<CreateProductResponse> CreateProductAsync(
        CreateProductRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/create");
        lazRequest.AddApiParameter("primary_category", request.PrimaryCategory.ToString(CultureInfo.InvariantCulture));
        if (request.SpuId is { } spu) lazRequest.AddApiParameter("spu_id", spu.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(request.GlobalSaleChannels)) lazRequest.AddApiParameter("global_sale_channels", request.GlobalSaleChannels);
        if (!string.IsNullOrEmpty(request.Attributes)) lazRequest.AddApiParameter("attributes", request.Attributes);
        if (!string.IsNullOrEmpty(request.Skus)) lazRequest.AddApiParameter("skus", request.Skus);
        if (!string.IsNullOrEmpty(request.Images)) lazRequest.AddApiParameter("images", request.Images);
        if (!string.IsNullOrEmpty(request.Product)) lazRequest.AddApiParameter("product", request.Product);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<CreateProductResponse>();
    }

    public async Task<CreateProductResponse> UpdateProductAsync(
        UpdateProductRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/update");
        lazRequest.AddApiParameter("item_id", request.ItemId.ToString(CultureInfo.InvariantCulture));
        if (request.PrimaryCategory is { } pc) lazRequest.AddApiParameter("primary_category", pc.ToString(CultureInfo.InvariantCulture));
        if (request.SpuId is { } spu) lazRequest.AddApiParameter("spu_id", spu.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(request.GlobalSaleChannels)) lazRequest.AddApiParameter("global_sale_channels", request.GlobalSaleChannels);
        if (!string.IsNullOrEmpty(request.Attributes)) lazRequest.AddApiParameter("attributes", request.Attributes);
        if (!string.IsNullOrEmpty(request.Skus)) lazRequest.AddApiParameter("skus", request.Skus);
        if (!string.IsNullOrEmpty(request.Images)) lazRequest.AddApiParameter("images", request.Images);
        if (!string.IsNullOrEmpty(request.Product)) lazRequest.AddApiParameter("product", request.Product);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<CreateProductResponse>();
    }

    public async Task<BaseProductResponse> DeactivateProductAsync(
        DeactivateProductRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/deactivate");
        lazRequest.AddApiParameter("item_id", request.ItemId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<BaseProductResponse>();
    }

    public async Task<GetProductsResponse> GetProductsAsync(
        GetProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/products/get") { HttpMethod = Constants.METHOD_GET };
        if (!string.IsNullOrEmpty(request.SellerSku)) lazRequest.AddApiParameter("seller_sku", request.SellerSku);
        if (!string.IsNullOrEmpty(request.ShopSku)) lazRequest.AddApiParameter("shop_sku", request.ShopSku);
        if (!string.IsNullOrEmpty(request.SkuSellerList)) lazRequest.AddApiParameter("sku_seller_list", request.SkuSellerList);
        if (request.Offset is { } off) lazRequest.AddApiParameter("offset", off.ToString(CultureInfo.InvariantCulture));
        if (request.Limit is { } lim) lazRequest.AddApiParameter("limit", lim.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(request.ItemIds)) lazRequest.AddApiParameter("item_id", request.ItemIds);
        if (!string.IsNullOrEmpty(request.ProductName)) lazRequest.AddApiParameter("product_name", request.ProductName);
        if (!string.IsNullOrEmpty(request.Options)) lazRequest.AddApiParameter("options", request.Options);
        if (!string.IsNullOrEmpty(request.CreatedAfter)) lazRequest.AddApiParameter("created_after", request.CreatedAfter);
        if (!string.IsNullOrEmpty(request.CreatedBefore)) lazRequest.AddApiParameter("created_before", request.CreatedBefore);
        if (!string.IsNullOrEmpty(request.UpdatedAfter)) lazRequest.AddApiParameter("updated_after", request.UpdatedAfter);
        if (!string.IsNullOrEmpty(request.UpdatedBefore)) lazRequest.AddApiParameter("updated_before", request.UpdatedBefore);
        if (!string.IsNullOrEmpty(request.Status)) lazRequest.AddApiParameter("status", request.Status);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetProductsResponse>();
    }

    public async Task<GetProductItemResponse> GetProductItemAsync(
        long itemId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/item/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("item_id", itemId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetProductItemResponse>();
    }

    public async Task<BaseProductResponse> AdjustSellableQuantityAsync(
        AdjustSellableQuantityRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/stock/sellable/adjust");
        lazRequest.AddApiParameter("item_id", request.ItemId.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("quantity", request.Quantity.ToString(CultureInfo.InvariantCulture));
        if (request.SkuId is { } sku) lazRequest.AddApiParameter("sku_id", sku.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(request.SellerSku)) lazRequest.AddApiParameter("seller_sku", request.SellerSku);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<BaseProductResponse>();
    }

    public async Task<BaseProductResponse> UpdateSellableQuantityAsync(
        UpdateSellableQuantityRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/stock/sellable/update");
        lazRequest.AddApiParameter("item_id", request.ItemId.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("quantity", request.Quantity.ToString(CultureInfo.InvariantCulture));
        if (request.SkuId is { } sku) lazRequest.AddApiParameter("sku_id", sku.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(request.SellerSku)) lazRequest.AddApiParameter("seller_sku", request.SellerSku);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<BaseProductResponse>();
    }

    public async Task<BaseProductResponse> UpdatePriceAsync(
        UpdatePriceRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/price/sellable/update");
        lazRequest.AddApiParameter("item_id", request.ItemId.ToString(CultureInfo.InvariantCulture));
        if (request.SkuId is { } sku) lazRequest.AddApiParameter("sku_id", sku.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(request.SellerSku)) lazRequest.AddApiParameter("seller_sku", request.SellerSku);
        if (!string.IsNullOrEmpty(request.Price)) lazRequest.AddApiParameter("price", request.Price);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<BaseProductResponse>();
    }

    public async Task<BaseProductResponse> SetImagesAsync(
        SetImagesRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/images/set");
        lazRequest.AddApiParameter("item_id", request.ItemId.ToString(CultureInfo.InvariantCulture));
        if (request.Images is { Count: > 0 })
        {
            lazRequest.AddApiParameter("images", string.Join(",", request.Images));
        }

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<BaseProductResponse>();
    }

    public async Task<GetCategoryTreeResponse> GetCategoryTreeAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/category/tree/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetCategoryTreeResponse>();
    }

    public async Task<GetCategoryAttributesResponse> GetCategoryAttributesAsync(
        long primaryCategoryId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/category/attributes/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("primary_category_id", primaryCategoryId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetCategoryAttributesResponse>();
    }

    public async Task<GetCategorySuggestionResponse> GetCategorySuggestionAsync(
        string productName,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(productName);

        var lazRequest = new LazRequest("/category/suggestion/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("product_name", productName);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetCategorySuggestionResponse>();
    }

    public async Task<GetCategorySuggestionBulkResponse> GetCategorySuggestionBulkAsync(
        GetCategorySuggestionBulkRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/category/suggestion/bulk/get") { HttpMethod = Constants.METHOD_GET };
        if (!string.IsNullOrEmpty(request.ProductNames)) lazRequest.AddApiParameter("product_names", request.ProductNames);
        if (!string.IsNullOrEmpty(request.Language)) lazRequest.AddApiParameter("language", request.Language);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetCategorySuggestionBulkResponse>();
    }

    public async Task<GetBrandsResponse> GetBrandsAsync(
        long categoryId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/brands/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("category_id", categoryId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetBrandsResponse>();
    }

    public async Task<BaseProductResponse> BatchUpdateSizeChartAsync(
        BatchUpdateSizeChartRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/size/chart/batch/update");
        if (!string.IsNullOrEmpty(request.SizeChartList)) lazRequest.AddApiParameter("size_chart_list", request.SizeChartList);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<BaseProductResponse>();
    }

    public async Task<GetQcStatusResponse> GetQcStatusAsync(
        long itemId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/qc/status/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("item_id", itemId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetQcStatusResponse>();
    }

    public async Task<GetQcAlertProductsResponse> GetQcAlertProductsAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/qc/alert/products/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetQcAlertProductsResponse>();
    }

    public async Task<GetPreQcRulesResponse> GetPreQcRulesAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/pre/qc/rules/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetPreQcRulesResponse>();
    }

    public async Task<GetProductResponseResponse> GetProductResponseAsync(
        long itemId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/response/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("item_id", itemId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetProductResponseResponse>();
    }

    public async Task<GetSellerItemLimitResponse> GetSellerItemLimitAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/seller/item/limit/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetSellerItemLimitResponse>();
    }

    public async Task<GetUnfilledAttributeItemResponse> GetUnfilledAttributeItemAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/unfilled/attribute/item/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetUnfilledAttributeItemResponse>();
    }

    public async Task<GetProductContentScoreResponse> GetProductContentScoreAsync(
        long itemId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/content/score/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("item_id", itemId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetProductContentScoreResponse>();
    }

    public async Task<ProductCheckResponse> ProductCheckAsync(
        ProductCheckRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/check");
        if (!string.IsNullOrEmpty(request.Product)) lazRequest.AddApiParameter("product", request.Product);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<ProductCheckResponse>();
    }

    public async Task<QueryProductExperimentConfigResponse> QueryProductExperimentConfigAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/experiment/config/query") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<QueryProductExperimentConfigResponse>();
    }

    public async Task<BaseProductResponse> ExitExperimentAsync(
        ExitExperimentRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/experiment/exit");
        if (!string.IsNullOrEmpty(request.ExperimentId)) lazRequest.AddApiParameter("experiment_id", request.ExperimentId);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<BaseProductResponse>();
    }

    public async Task<GetNextCascadePropResponse> GetNextCascadePropAsync(
        long categoryId,
        long? parentPropId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/next/cascade/prop/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("category_id", categoryId.ToString(CultureInfo.InvariantCulture));
        if (parentPropId is { } ppi) lazRequest.AddApiParameter("parent_prop_id", ppi.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetNextCascadePropResponse>();
    }
}
