using Laz.Sdk.Models.Store;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class StoreServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string CustomPageSuccessBody = """
    {
      "code":"0",
      "data":{
        "total":"10",
        "currentPage":"1",
        "pageSize":"5",
        "list":[
          {
            "page_id":"1001",
            "page_name":"Homepage Banner",
            "page_type":"banner",
            "page_url":"https://example.com/page/1001",
            "created_at":"2024-01-01 10:00:00",
            "updated_at":"2024-01-15 12:00:00"
          },
          {
            "page_id":"1002",
            "page_name":"Promo Section",
            "page_type":"promo",
            "page_url":"https://example.com/page/1002",
            "created_at":"2024-01-05 08:00:00",
            "updated_at":"2024-01-20 14:00:00"
          }
        ]
      },
      "request_id":"req-scp-001"
    }
    """;

    // ─── GetStoreCustomPageAsync ────────────────────────────────────────

    [Fact]
    public async Task GetStoreCustomPageAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(CustomPageSuccessBody);
        var client = NewClient(handler);

        var response = await client.Store.GetStoreCustomPageAsync(
            page: 1,
            size: 5,
            keyword: null,
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("req-scp-001", response.RequestId);
        Assert.NotNull(response.Data);
        Assert.Equal(10, response.Data!.Total);
        Assert.Equal(1, response.Data.CurrentPage);
        Assert.Equal(5, response.Data.PageSize);
        Assert.NotNull(response.Data.List);
        Assert.Equal(2, response.Data.List!.Count);
        Assert.Equal(1001, response.Data.List[0].PageId);
        Assert.Equal("Homepage Banner", response.Data.List[0].PageName);
        Assert.Equal("banner", response.Data.List[0].PageType);
        Assert.Equal("https://example.com/page/1001", response.Data.List[0].PageUrl);
    }

    [Fact]
    public async Task GetStoreCustomPageAsync_BuildsGetUrl_WithRequiredParams()
    {
        var handler = new TestHandler(CustomPageSuccessBody);
        var client = NewClient(handler);

        await client.Store.GetStoreCustomPageAsync(page: 1, size: 10, keyword: null, accessToken: "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/store/custom/page/get?", url, StringComparison.Ordinal);
        Assert.Contains("page=1", url, StringComparison.Ordinal);
        Assert.Contains("size=10", url, StringComparison.Ordinal);
        Assert.DoesNotContain("keyword=", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetStoreCustomPageAsync_IncludesKeyword()
    {
        var handler = new TestHandler(CustomPageSuccessBody);
        var client = NewClient(handler);

        await client.Store.GetStoreCustomPageAsync(page: 1, size: 10, keyword: "banner", accessToken: "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("keyword=banner", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetStoreCustomPageAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"400","type":"ISV","message":"Invalid parameters","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Store.GetStoreCustomPageAsync(1, 10, null, "tok"));
        Assert.Equal("400", ex.ErrorCode);
    }

    [Fact]
    public async Task GetStoreCustomPageAsync_RejectsZeroPage()
    {
        var client = NewClient(new TestHandler(CustomPageSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Store.GetStoreCustomPageAsync(0, 10, null, "tok"));
    }

    [Fact]
    public async Task GetStoreCustomPageAsync_RejectsZeroSize()
    {
        var client = NewClient(new TestHandler(CustomPageSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Store.GetStoreCustomPageAsync(1, 0, null, "tok"));
    }

    [Fact]
    public async Task GetStoreCustomPageAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(CustomPageSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Store.GetStoreCustomPageAsync(1, 10, null, ""));
    }
}
