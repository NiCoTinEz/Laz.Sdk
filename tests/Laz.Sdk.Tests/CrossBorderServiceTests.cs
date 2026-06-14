using Laz.Sdk.Models.CrossBorder;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class CrossBorderServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string CreateSuccessBody = """
    {
      "code":"0",
      "data":{
        "sku_list":[
          {"seller_sku":"SKU-001","sku_id":"1001","shop_sku":"SHOP-SKU-001"},
          {"seller_sku":"SKU-002","sku_id":"1002","shop_sku":"SHOP-SKU-002"}
        ]
      },
      "request_id":"req-cgp-001"
    }
    """;

    private const string ExtensionSuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "global_item_id":"5001",
          "item_id":"3001",
          "extensions":[
            {"name":"weight","value":"1.5"},
            {"name":"length","value":"30"}
          ]
        }
      ],
      "request_id":"req-ext-001"
    }
    """;

    private const string UpdateSkuSuccessBody = """
    {
      "code":"0",
      "data":{
        "sku_list":[
          {"seller_sku":"SKU-001","sku_id":"1001","shop_sku":"SHOP-SKU-001"}
        ]
      },
      "request_id":"req-usk-001"
    }
    """;

    private const string SellerStatusSuccessBody = """
    {
      "code":"0",
      "data":{
        "cross_border_enabled":"true"
      },
      "request_id":"req-sts-001"
    }
    """;

    // ─── CreateGlobalProductAsync ───────────────────────────────────────

    [Fact]
    public async Task CreateGlobalProductAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(CreateSuccessBody);
        var client = NewClient(handler);

        var response = await client.CrossBorder.CreateGlobalProductAsync(
            "<Product>...</Product>",
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("req-cgp-001", response.RequestId);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data!.SkuList);
        Assert.Equal(2, response.Data.SkuList!.Count);
        Assert.Equal("SKU-001", response.Data.SkuList[0].SellerSku);
        Assert.Equal(1001, response.Data.SkuList[0].SkuId);
        Assert.Equal("SHOP-SKU-001", response.Data.SkuList[0].ShopSku);
    }

    [Fact]
    public async Task CreateGlobalProductAsync_PostsPayload()
    {
        var handler = new TestHandler(CreateSuccessBody);
        var client = NewClient(handler);

        await client.CrossBorder.CreateGlobalProductAsync("<Product>test</Product>", "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("payload=%3CProduct%3Etest%3C%2FProduct%3E", body);
        Assert.Contains("access_token=tok", body);
    }

    [Fact]
    public async Task CreateGlobalProductAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"400","type":"ISV","message":"Invalid payload","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.CrossBorder.CreateGlobalProductAsync("<Product/>", "tok"));
        Assert.Equal("400", ex.ErrorCode);
    }

    [Fact]
    public async Task CreateGlobalProductAsync_RejectsEmptyPayload()
    {
        var client = NewClient(new TestHandler(CreateSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CrossBorder.CreateGlobalProductAsync("", "tok"));
    }

    [Fact]
    public async Task CreateGlobalProductAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(CreateSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CrossBorder.CreateGlobalProductAsync("<Product/>", ""));
    }

    // ─── GetGlobalProductExtensionAsync ─────────────────────────────────

    [Fact]
    public async Task GetGlobalProductExtensionAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(ExtensionSuccessBody);
        var client = NewClient(handler);

        var response = await client.CrossBorder.GetGlobalProductExtensionAsync(
            globalItemIds: new[] { 5001L },
            itemIds: null,
            country: "sg",
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        var ext = Assert.Single(response.Data!);
        Assert.Equal(5001, ext.GlobalItemId);
        Assert.Equal(3001, ext.ItemId);
        Assert.NotNull(ext.Extensions);
        Assert.Equal(2, ext.Extensions!.Count);
        Assert.Equal("weight", ext.Extensions[0].Name);
        Assert.Equal("1.5", ext.Extensions[0].Value);
    }

    [Fact]
    public async Task GetGlobalProductExtensionAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(ExtensionSuccessBody);
        var client = NewClient(handler);

        await client.CrossBorder.GetGlobalProductExtensionAsync(
            globalItemIds: new[] { 5001L, 5002L },
            itemIds: new[] { 3001L },
            country: "my",
            accessToken: "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/product/global/extension?", url, StringComparison.Ordinal);
        Assert.Contains("global_item_ids=5001%2C5002", url, StringComparison.Ordinal);
        Assert.Contains("item_ids=3001", url, StringComparison.Ordinal);
        Assert.Contains("country=my", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetGlobalProductExtensionAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"400","type":"ISV","message":"Bad request","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.CrossBorder.GetGlobalProductExtensionAsync(null, null, "sg", "tok"));
        Assert.Equal("400", ex.ErrorCode);
    }

    [Fact]
    public async Task GetGlobalProductExtensionAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(ExtensionSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CrossBorder.GetGlobalProductExtensionAsync(null, null, "sg", ""));
    }

    [Fact]
    public async Task GetGlobalProductExtensionAsync_RejectsEmptyCountry()
    {
        var client = NewClient(new TestHandler(ExtensionSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CrossBorder.GetGlobalProductExtensionAsync(null, null, "", "tok"));
    }

    // ─── UpdateGlobalSkuAsync ───────────────────────────────────────────

    [Fact]
    public async Task UpdateGlobalSkuAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(UpdateSkuSuccessBody);
        var client = NewClient(handler);

        var response = await client.CrossBorder.UpdateGlobalSkuAsync(
            "<SKU>...</SKU>",
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data!.SkuList);
        var sku = Assert.Single(response.Data.SkuList!);
        Assert.Equal("SKU-001", sku.SellerSku);
        Assert.Equal(1001, sku.SkuId);
    }

    [Fact]
    public async Task UpdateGlobalSkuAsync_PostsPayload()
    {
        var handler = new TestHandler(UpdateSkuSuccessBody);
        var client = NewClient(handler);

        await client.CrossBorder.UpdateGlobalSkuAsync("<SKU>test</SKU>", "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("payload=%3CSKU%3Etest%3C%2FSKU%3E", body);
    }

    [Fact]
    public async Task UpdateGlobalSkuAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"500","type":"ISV","message":"Server error","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.CrossBorder.UpdateGlobalSkuAsync("<SKU/>", "tok"));
        Assert.Equal("500", ex.ErrorCode);
    }

    [Fact]
    public async Task UpdateGlobalSkuAsync_RejectsEmptyPayload()
    {
        var client = NewClient(new TestHandler(UpdateSkuSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CrossBorder.UpdateGlobalSkuAsync("", "tok"));
    }

    [Fact]
    public async Task UpdateGlobalSkuAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(UpdateSkuSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CrossBorder.UpdateGlobalSkuAsync("<SKU/>", ""));
    }

    // ─── GetGlobalSellerStatusAsync ─────────────────────────────────────

    [Fact]
    public async Task GetGlobalSellerStatusAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(SellerStatusSuccessBody);
        var client = NewClient(handler);

        var response = await client.CrossBorder.GetGlobalSellerStatusAsync(accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("true", response.Data!.CrossBorderEnabled);
    }

    [Fact]
    public async Task GetGlobalSellerStatusAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(SellerStatusSuccessBody);
        var client = NewClient(handler);

        await client.CrossBorder.GetGlobalSellerStatusAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/product/global/seller/status?", url, StringComparison.Ordinal);
        Assert.Contains("access_token=tok", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetGlobalSellerStatusAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"403","type":"ISV","message":"Forbidden","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.CrossBorder.GetGlobalSellerStatusAsync("tok"));
        Assert.Equal("403", ex.ErrorCode);
    }

    [Fact]
    public async Task GetGlobalSellerStatusAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(SellerStatusSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CrossBorder.GetGlobalSellerStatusAsync(""));
    }
}
