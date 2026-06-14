using Laz.Sdk.Models.Products;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class ProductServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string SuccessBody = """"
    {
      "code":"0",
      "data":{
        "item_id":"12345",
        "sku_id":"67890",
        "seller_sku":"TEST-SKU-001"
      },
      "request_id":"req-abc-123"
    }
    """";

    private const string ProductsListBody = """"
    {
      "code":"0",
      "data":{
        "total_products":"2",
        "products_count":"2",
        "products":[
          {
            "item_id":"1001",
            "primary_category":"123",
            "item_name":"Test Product 1",
            "status":"active",
            "images":["https://example.com/img1.jpg"],
            "created_time":"2025-01-01 00:00:00",
            "updated_time":"2025-01-02 00:00:00"
          },
          {
            "item_id":"1002",
            "primary_category":"456",
            "item_name":"Test Product 2",
            "status":"inactive",
            "images":["https://example.com/img2.jpg"],
            "created_time":"2025-01-03 00:00:00",
            "updated_time":"2025-01-04 00:00:00"
          }
        ]
      },
      "request_id":"req-prod-list"
    }
    """";

    private const string CategoryTreeBody = """"
    {
      "code":"0",
      "data":[
        {
          "category_id":"100",
          "name":"Electronics",
          "var":true,
          "leaf":false,
          "enabled":true,
          "children":[
            {
              "category_id":"101",
              "name":"Mobile Phones",
              "var":false,
              "leaf":true,
              "enabled":true
            }
          ]
        }
      ],
      "request_id":"req-cat-tree"
    }
    """";

    private const string CategoryAttributesBody = """"
    {
      "code":"0",
      "data":[
        {
          "id":"101",
          "name":"brand",
          "label":"Brand",
          "attribute_type":"single_select",
          "input_type":"select",
          "is_mandatory":true,
          "is_sale_prop":false,
          "is_variant":false,
          "options":[
            {"name":"Apple","label":"Apple"},
            {"name":"Samsung","label":"Samsung"}
          ]
        }
      ],
      "request_id":"req-cat-attrs"
    }
    """";

    private const string BrandsBody = """"
    {
      "code":"0",
      "data":[
        {
          "brand_id":"1001",
          "name_en":"Apple",
          "global_brand_id":"5001"
        }
      ],
      "request_id":"req-brands"
    }
    """";

    private const string CategorySuggestionBody = """"
    {
      "code":"0",
      "data":[
        {
          "category_id":"101",
          "category_name":"Mobile Phones",
          "leaf":true,
          "confidence":"0.95"
        }
      ],
      "request_id":"req-cat-sug"
    }
    """";

    private const string QcStatusBody = """"
    {
      "code":"0",
      "data":[
        {
          "item_id":"12345",
          "seller_sku":"TEST-SKU",
          "status":"approved",
          "reason":""
        }
      ],
      "request_id":"req-qc"
    }
    """";

    private const string ProductItemBody = """"
    {
      "code":"0",
      "data":{
        "item_id":"12345",
        "primary_category":"101",
        "item_name":"Test Product",
        "status":"active",
        "images":["https://example.com/img.jpg"],
        "created_time":"2025-01-01 00:00:00",
        "updated_time":"2025-01-02 00:00:00"
      },
      "request_id":"req-item"
    }
    """";

    // ──────────────────────────────────────────────
    // CreateProduct
    // ──────────────────────────────────────────────

    [Fact]
    public async Task CreateProductAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        var response = await client.Products.CreateProductAsync(
            new CreateProductRequest
            {
                PrimaryCategory = 123,
                Attributes = "{\"name\":\"Test\"}",
                Skus = "[{\"seller_sku\":\"SKU001\",\"quantity\":10}]",
            },
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("12345", response.Data?.ItemId?.ToString());
        Assert.Equal("67890", response.Data?.SkuId?.ToString());
        Assert.Equal("TEST-SKU-001", response.Data?.SellerSku);
    }

    [Fact]
    public async Task CreateProductAsync_BuildsPostRequest()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.CreateProductAsync(
            new CreateProductRequest
            {
                PrimaryCategory = 123,
                Attributes = "{\"name\":\"Test\"}",
                Skus = "[{\"seller_sku\":\"SKU001\"}]",
            },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("primary_category=123", body);
        Assert.Contains("attributes=" + Uri.EscapeDataString("{\"name\":\"Test\"}"), body);
        Assert.Contains("access_token=tok", body);
    }

    [Fact]
    public async Task CreateProductAsync_ThrowsOnError()
    {
        const string ErrorBody = """{"code":"1001","type":"ISV","message":"Invalid category","request_id":"err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Products.CreateProductAsync(
                new CreateProductRequest { PrimaryCategory = 999 }, "tok"));

        Assert.Equal("1001", ex.ErrorCode);
    }

    [Fact]
    public async Task CreateProductAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Products.CreateProductAsync(
                new CreateProductRequest { PrimaryCategory = 123 }, ""));
    }

    // ──────────────────────────────────────────────
    // UpdateProduct
    // ──────────────────────────────────────────────

    [Fact]
    public async Task UpdateProductAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        var response = await client.Products.UpdateProductAsync(
            new UpdateProductRequest { ItemId = 12345, Attributes = "{\"name\":\"Updated\"}" },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("12345", response.Data?.ItemId?.ToString());
    }

    [Fact]
    public async Task UpdateProductAsync_BuildsPostRequest()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.UpdateProductAsync(
            new UpdateProductRequest { ItemId = 12345, Attributes = "{\"name\":\"Updated\"}" },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("item_id=12345", body);
        Assert.Contains("attributes=" + Uri.EscapeDataString("{\"name\":\"Updated\"}"), body);
    }

    // ──────────────────────────────────────────────
    // DeactivateProduct
    // ──────────────────────────────────────────────

    [Fact]
    public async Task DeactivateProductAsync_SendsItemId()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.DeactivateProductAsync(
            new DeactivateProductRequest { ItemId = 12345 }, "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("item_id=12345", handler.LastRequestBody!);
    }

    // ──────────────────────────────────────────────
    // GetProducts
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetProductsAsync_ParsesListResponse()
    {
        var handler = new TestHandler(ProductsListBody);
        var client = NewClient(handler);

        var response = await client.Products.GetProductsAsync(
            new GetProductsRequest { Limit = 10 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data?.Products);
        Assert.Equal(2, response.Data.Products.Count);
        Assert.Equal("Test Product 1", response.Data.Products[0].ItemName);
        Assert.Equal("active", response.Data.Products[0].Status);
    }

    [Fact]
    public async Task GetProductsAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(ProductsListBody);
        var client = NewClient(handler);

        await client.Products.GetProductsAsync(
            new GetProductsRequest { Limit = 10, Offset = 5, Status = "active" }, "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("limit=10", url);
        Assert.Contains("offset=5", url);
        Assert.Contains("status=active", url);
        Assert.Contains("access_token=tok", url);
    }

    // ──────────────────────────────────────────────
    // GetProductItem
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetProductItemAsync_ParsesResponse()
    {
        var handler = new TestHandler(ProductItemBody);
        var client = NewClient(handler);

        var response = await client.Products.GetProductItemAsync(12345, "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("Test Product", response.Data?.ItemName);
        Assert.Equal("active", response.Data?.Status);
    }

    [Fact]
    public async Task GetProductItemAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(ProductItemBody);
        var client = NewClient(handler);

        await client.Products.GetProductItemAsync(12345, "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("item_id=12345", url);
    }

    // ──────────────────────────────────────────────
    // AdjustSellableQuantity
    // ──────────────────────────────────────────────

    [Fact]
    public async Task AdjustSellableQuantityAsync_SendsQuantity()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.AdjustSellableQuantityAsync(
            new AdjustSellableQuantityRequest { ItemId = 12345, Quantity = 5 }, "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("item_id=12345", body);
        Assert.Contains("quantity=5", body);
    }

    // ──────────────────────────────────────────────
    // UpdateSellableQuantity
    // ──────────────────────────────────────────────

    [Fact]
    public async Task UpdateSellableQuantityAsync_SendsAbsoluteQuantity()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.UpdateSellableQuantityAsync(
            new UpdateSellableQuantityRequest { ItemId = 12345, Quantity = 100 }, "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("item_id=12345", body);
        Assert.Contains("quantity=100", body);
    }

    // ──────────────────────────────────────────────
    // UpdatePrice
    // ──────────────────────────────────────────────

    [Fact]
    public async Task UpdatePriceAsync_SendsPrice()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.UpdatePriceAsync(
            new UpdatePriceRequest { ItemId = 12345, Price = "99.99" }, "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("item_id=12345", body);
        Assert.Contains("price=99.99", body);
    }

    // ──────────────────────────────────────────────
    // SetImages
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SetImagesAsync_SendsImageUrls()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.SetImagesAsync(
            new SetImagesRequest
            {
                ItemId = 12345,
                Images = new[] { "https://example.com/img1.jpg", "https://example.com/img2.jpg" },
            },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("item_id=12345", body);
        Assert.Contains("images=" + Uri.EscapeDataString("https://example.com/img1.jpg,https://example.com/img2.jpg"), body);
    }

    // ──────────────────────────────────────────────
    // GetCategoryTree
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetCategoryTreeAsync_ParsesResponse()
    {
        var handler = new TestHandler(CategoryTreeBody);
        var client = NewClient(handler);

        var response = await client.Products.GetCategoryTreeAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("Electronics", response.Data[0].Name);
        Assert.NotNull(response.Data[0].Children);
        Assert.Equal("Mobile Phones", response.Data[0].Children![0].Name);
    }

    [Fact]
    public async Task GetCategoryTreeAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(CategoryTreeBody);
        var client = NewClient(handler);

        await client.Products.GetCategoryTreeAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("/category/tree/get", handler.LastRequest!.RequestUri!.ToString());
    }

    // ──────────────────────────────────────────────
    // GetCategoryAttributes
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetCategoryAttributesAsync_ParsesResponse()
    {
        var handler = new TestHandler(CategoryAttributesBody);
        var client = NewClient(handler);

        var response = await client.Products.GetCategoryAttributesAsync(101, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("brand", response.Data[0].Name);
        Assert.True(response.Data[0].IsMandatory);
        Assert.NotNull(response.Data[0].Options);
        Assert.Equal(2, response.Data[0].Options!.Count);
    }

    [Fact]
    public async Task GetCategoryAttributesAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(CategoryAttributesBody);
        var client = NewClient(handler);

        await client.Products.GetCategoryAttributesAsync(101, "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("primary_category_id=101", url);
    }

    // ──────────────────────────────────────────────
    // GetCategorySuggestion
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetCategorySuggestionAsync_ParsesResponse()
    {
        var handler = new TestHandler(CategorySuggestionBody);
        var client = NewClient(handler);

        var response = await client.Products.GetCategorySuggestionAsync("Mobile Phone", "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("Mobile Phones", response.Data[0].CategoryName);
    }

    [Fact]
    public async Task GetCategorySuggestionAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(CategorySuggestionBody);
        var client = NewClient(handler);

        await client.Products.GetCategorySuggestionAsync("Mobile Phone", "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("product_name", url);
        Assert.Contains("access_token=tok", url);
        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
    }

    [Fact]
    public async Task GetCategorySuggestionAsync_RejectsEmptyProductName()
    {
        var client = NewClient(new TestHandler(CategorySuggestionBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Products.GetCategorySuggestionAsync("", "tok"));
    }

    // ──────────────────────────────────────────────
    // GetCategorySuggestionBulk
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetCategorySuggestionBulkAsync_SendsProductNames()
    {
        var handler = new TestHandler("""{"code":"0","data":[],"request_id":"r"}""");
        var client = NewClient(handler);

        await client.Products.GetCategorySuggestionBulkAsync(
            new GetCategorySuggestionBulkRequest
            {
                ProductNames = "[\"Phone\",\"Laptop\"]",
                Language = "en_US",
            },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("product_names", url);
        Assert.Contains("language=en_US", url);
        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
    }

    // ──────────────────────────────────────────────
    // GetBrands
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetBrandsAsync_ParsesResponse()
    {
        var handler = new TestHandler(BrandsBody);
        var client = NewClient(handler);

        var response = await client.Products.GetBrandsAsync(101, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("Apple", response.Data[0].NameEn);
    }

    [Fact]
    public async Task GetBrandsAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(BrandsBody);
        var client = NewClient(handler);

        await client.Products.GetBrandsAsync(101, "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("category_id=101", url);
    }

    // ──────────────────────────────────────────────
    // BatchUpdateSizeChart
    // ──────────────────────────────────────────────

    [Fact]
    public async Task BatchUpdateSizeChartAsync_SendsSizeChartList()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.BatchUpdateSizeChartAsync(
            new BatchUpdateSizeChartRequest { SizeChartList = "[{\"item_id\":123}]" }, "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("size_chart_list=" + Uri.EscapeDataString("[{\"item_id\":123}]"), body);
    }

    // ──────────────────────────────────────────────
    // GetQcStatus
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetQcStatusAsync_ParsesResponse()
    {
        var handler = new TestHandler(QcStatusBody);
        var client = NewClient(handler);

        var response = await client.Products.GetQcStatusAsync(12345, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("approved", response.Data[0].Status);
    }

    [Fact]
    public async Task GetQcStatusAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(QcStatusBody);
        var client = NewClient(handler);

        await client.Products.GetQcStatusAsync(12345, "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("item_id=12345", url);
    }

    // ──────────────────────────────────────────────
    // GetQcAlertProducts
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetQcAlertProductsAsync_BuildsGetUrl()
    {
        const string body = """{"code":"0","data":{"alert_products":[]},"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        await client.Products.GetQcAlertProductsAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("/product/qc/alert/products/get", handler.LastRequest!.RequestUri!.ToString());
    }

    // ──────────────────────────────────────────────
    // GetPreQcRules
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetPreQcRulesAsync_BuildsGetUrl()
    {
        const string body = """{"code":"0","data":[],"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        await client.Products.GetPreQcRulesAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("/product/pre/qc/rules/get", handler.LastRequest!.RequestUri!.ToString());
    }

    // ──────────────────────────────────────────────
    // GetProductResponse
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetProductResponseAsync_BuildsGetUrl()
    {
        const string body = """{"code":"0","data":{"item_id":"12345","status":"approved"},"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        var response = await client.Products.GetProductResponseAsync(12345, "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("approved", response.Data?.Status);
        Assert.Contains("item_id=12345", handler.LastRequest!.RequestUri!.ToString());
    }

    // ──────────────────────────────────────────────
    // GetSellerItemLimit
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetSellerItemLimitAsync_ParsesResponse()
    {
        const string body = """{"code":"0","data":{"item_limit":"1000","item_count":"50","remaining":"950"},"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        var response = await client.Products.GetSellerItemLimitAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.Equal(1000, response.Data?.ItemLimit);
        Assert.Equal(50, response.Data?.ItemCount);
        Assert.Equal(950, response.Data?.Remaining);
    }

    // ──────────────────────────────────────────────
    // GetUnfilledAttributeItem
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetUnfilledAttributeItemAsync_BuildsGetUrl()
    {
        const string body = """{"code":"0","data":[],"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        await client.Products.GetUnfilledAttributeItemAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("/product/unfilled/attribute/item/get", handler.LastRequest!.RequestUri!.ToString());
    }

    // ──────────────────────────────────────────────
    // GetProductContentScore
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetProductContentScoreAsync_ParsesResponse()
    {
        const string body = """{"code":"0","data":{"item_id":"12345","score":"85","details":"Good content"},"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        var response = await client.Products.GetProductContentScoreAsync(12345, "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal(85, response.Data?.Score);
        Assert.Equal("Good content", response.Data?.Details);
    }

    // ──────────────────────────────────────────────
    // ProductCheck
    // ──────────────────────────────────────────────

    [Fact]
    public async Task ProductCheckAsync_SendsProduct()
    {
        const string body = """{"code":"0","data":{"check_result":"pass"},"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        var response = await client.Products.ProductCheckAsync(
            new ProductCheckRequest { Product = "<Product><ItemName>Test</ItemName></Product>" }, "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("pass", response.Data?.CheckResult);
        Assert.Contains("product=" + Uri.EscapeDataString("<Product><ItemName>Test</ItemName></Product>"), handler.LastRequestBody!);
    }

    // ──────────────────────────────────────────────
    // QueryProductExperimentConfig
    // ──────────────────────────────────────────────

    [Fact]
    public async Task QueryProductExperimentConfigAsync_BuildsGetUrl()
    {
        const string body = """{"code":"0","data":[],"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        await client.Products.QueryProductExperimentConfigAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("/product/experiment/config/query", handler.LastRequest!.RequestUri!.ToString());
    }

    // ──────────────────────────────────────────────
    // ExitExperiment
    // ──────────────────────────────────────────────

    [Fact]
    public async Task ExitExperimentAsync_SendsExperimentId()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Products.ExitExperimentAsync(
            new ExitExperimentRequest { ExperimentId = "exp-123" }, "tok");

        Assert.Contains("experiment_id=exp-123", handler.LastRequestBody!);
    }

    // ──────────────────────────────────────────────
    // GetNextCascadeProp
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetNextCascadePropAsync_BuildsGetUrl()
    {
        const string body = """{"code":"0","data":[],"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        await client.Products.GetNextCascadePropAsync(101, 5001, "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("category_id=101", url);
        Assert.Contains("parent_prop_id=5001", url);
    }

    [Fact]
    public async Task GetNextCascadePropAsync_OmitsParentPropId_WhenNull()
    {
        const string body = """{"code":"0","data":[],"request_id":"r"}""";
        var handler = new TestHandler(body);
        var client = NewClient(handler);

        await client.Products.GetNextCascadePropAsync(101, null, "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("category_id=101", url);
        Assert.DoesNotContain("parent_prop_id", url);
    }

    // ──────────────────────────────────────────────
    // Regional gateway
    // ──────────────────────────────────────────────

    [Fact]
    public async Task CreateProductAsync_RespectsRegionalGateway()
    {
        var handler = new TestHandler(SuccessBody);
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as", ServerUrl = UrlConstants.API_GATEWAY_URL_TH };
        var http = new HttpClient(handler);
        var client = new LazClient(http, opts);

        await client.Products.CreateProductAsync(
            new CreateProductRequest { PrimaryCategory = 123 }, "tok");

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_TH + "/product/create",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetProductsAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(ProductsListBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Products.GetProductsAsync(new GetProductsRequest(), ""));
    }

    [Fact]
    public async Task GetProductItemAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(ProductItemBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Products.GetProductItemAsync(12345, ""));
    }

    [Fact]
    public async Task GetCategoryTreeAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(CategoryTreeBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Products.GetCategoryTreeAsync(""));
    }
}
