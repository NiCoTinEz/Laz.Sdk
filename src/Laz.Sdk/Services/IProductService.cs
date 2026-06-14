using Laz.Sdk.Models.Products;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the <c>/product/*</c>, <c>/products/*</c>, <c>/category/*</c>, <c>/brands/*</c>,
/// <c>/price/*</c>, <c>/images/*</c>, and <c>/size/*</c> families of Lazada Open Platform endpoints.
/// Access via <c>ILazClient.Products</c>.
/// </summary>
public interface IProductService
{
    /// <summary>Create a new product. Calls <c>/product/create</c>.</summary>
    Task<CreateProductResponse> CreateProductAsync(
        CreateProductRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update an existing product. Calls <c>/product/update</c>.</summary>
    Task<CreateProductResponse> UpdateProductAsync(
        UpdateProductRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Deactivate a product. Calls <c>/product/deactivate</c>.</summary>
    Task<BaseProductResponse> DeactivateProductAsync(
        DeactivateProductRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>List products with filters. Calls <c>/products/get</c>.</summary>
    Task<GetProductsResponse> GetProductsAsync(
        GetProductsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get a single product item by id. Calls <c>/product/item/get</c>.</summary>
    Task<GetProductItemResponse> GetProductItemAsync(
        long itemId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Adjust sellable stock quantity (increment/decrement). Calls <c>/product/stock/sellable/adjust</c>.</summary>
    Task<BaseProductResponse> AdjustSellableQuantityAsync(
        AdjustSellableQuantityRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update sellable stock quantity (absolute value). Calls <c>/product/stock/sellable/update</c>.</summary>
    Task<BaseProductResponse> UpdateSellableQuantityAsync(
        UpdateSellableQuantityRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Update sellable price. Calls <c>/price/sellable/update</c>.</summary>
    Task<BaseProductResponse> UpdatePriceAsync(
        UpdatePriceRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Set product images. Calls <c>/images/set</c>.</summary>
    Task<BaseProductResponse> SetImagesAsync(
        SetImagesRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get the category tree. Calls <c>/category/tree/get</c>.</summary>
    Task<GetCategoryTreeResponse> GetCategoryTreeAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get category attributes. Calls <c>/category/attributes/get</c>.</summary>
    Task<GetCategoryAttributesResponse> GetCategoryAttributesAsync(
        long primaryCategoryId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Suggest categories for a product name. Calls <c>/category/suggestion/get</c>.</summary>
    Task<GetCategorySuggestionResponse> GetCategorySuggestionAsync(
        string productName,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Bulk suggest categories. Calls <c>/category/suggestion/bulk/get</c>.</summary>
    Task<GetCategorySuggestionBulkResponse> GetCategorySuggestionBulkAsync(
        GetCategorySuggestionBulkRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get brands for a category. Calls <c>/brands/get</c>.</summary>
    Task<GetBrandsResponse> GetBrandsAsync(
        long categoryId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Batch update size charts. Calls <c>/size/chart/batch/update</c>.</summary>
    Task<BaseProductResponse> BatchUpdateSizeChartAsync(
        BatchUpdateSizeChartRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get QC status for a product. Calls <c>/product/qc/status/get</c>.</summary>
    Task<GetQcStatusResponse> GetQcStatusAsync(
        long itemId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get QC alert products. Calls <c>/product/qc/alert/products/get</c>.</summary>
    Task<GetQcAlertProductsResponse> GetQcAlertProductsAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get pre-QC rules. Calls <c>/product/pre/qc/rules/get</c>.</summary>
    Task<GetPreQcRulesResponse> GetPreQcRulesAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get product response (creation/update result). Calls <c>/product/response/get</c>.</summary>
    Task<GetProductResponseResponse> GetProductResponseAsync(
        long itemId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get seller item limit. Calls <c>/product/seller/item/limit/get</c>.</summary>
    Task<GetSellerItemLimitResponse> GetSellerItemLimitAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get unfilled attribute items. Calls <c>/product/unfilled/attribute/item/get</c>.</summary>
    Task<GetUnfilledAttributeItemResponse> GetUnfilledAttributeItemAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get product content score. Calls <c>/product/content/score/get</c>.</summary>
    Task<GetProductContentScoreResponse> GetProductContentScoreAsync(
        long itemId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Check a product before creation. Calls <c>/product/check</c>.</summary>
    Task<ProductCheckResponse> ProductCheckAsync(
        ProductCheckRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Query product experiment config. Calls <c>/product/experiment/config/query</c>.</summary>
    Task<QueryProductExperimentConfigResponse> QueryProductExperimentConfigAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Exit a product experiment. Calls <c>/product/experiment/exit</c>.</summary>
    Task<BaseProductResponse> ExitExperimentAsync(
        ExitExperimentRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>Get next cascade property. Calls <c>/product/next/cascade/prop/get</c>.</summary>
    Task<GetNextCascadePropResponse> GetNextCascadePropAsync(
        long categoryId,
        long? parentPropId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
