using System.Net.Http;
using Laz.Sdk.Util;
using Microsoft.Extensions.Options;
using Xunit;

namespace Laz.Sdk.Tests;

public class LazClientTests
{
    private static LazClient NewClient(TestHandler handler, Action<LazClientOptions>? configure = null)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        configure?.Invoke(opts);
        var http = new HttpClient(handler) { BaseAddress = null };
        return new LazClient(http, opts);
    }

    [Fact]
    public async Task ExecuteAsync_Get_BuildsQueryString_AndParsesResponse()
    {
        var handler = new TestHandler("""{"code":"0","type":"","message":"ok","request_id":"req-1","data":{}}""");
        var client = NewClient(handler);

        var request = new LazRequest("/system/ping") { HttpMethod = Constants.METHOD_GET };
        request.AddApiParameter("foo", "bar");

        var response = await client.ExecuteAsync(request);

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/system/ping?", url, StringComparison.Ordinal);
        Assert.Contains("app_key=ak", url, StringComparison.Ordinal);
        Assert.Contains("sign=",      url, StringComparison.Ordinal);
        Assert.Contains("timestamp=", url, StringComparison.Ordinal);
        Assert.Contains("foo=bar",    url, StringComparison.Ordinal);

        Assert.False(response.IsError());
        Assert.Equal("0",     response.Code);
        Assert.Equal("ok",    response.Message);
        Assert.Equal("req-1", response.RequestId);
    }

    [Fact]
    public async Task ExecuteAsync_Post_NoFiles_SendsFormUrlEncoded()
    {
        var handler = new TestHandler("""{"code":"0"}""");
        var client = NewClient(handler);

        var request = new LazRequest("/orders/get"); // default POST
        request.AddApiParameter("limit", "10");

        await client.ExecuteAsync(request);

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Equal("application/x-www-form-urlencoded", handler.LastRequest.Content!.Headers.ContentType!.MediaType);
        Assert.Contains("limit=10",  handler.LastRequestBody!, StringComparison.Ordinal);
        Assert.Contains("app_key=ak", handler.LastRequestBody!, StringComparison.Ordinal);
        Assert.Contains("sign=",      handler.LastRequestBody!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_Post_WithFile_SendsMultipart()
    {
        var handler = new TestHandler("""{"code":"0"}""");
        var client = NewClient(handler);

        var request = new LazRequest("/image/upload");
        request.AddFileParameter("image", new FileItem("photo.jpg", new byte[] { 0x01, 0x02, 0x03 }, "image/jpeg"));

        await client.ExecuteAsync(request);

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var ctype = handler.LastRequest.Content!.Headers.ContentType!;
        Assert.Equal("multipart/form-data", ctype.MediaType);
        Assert.NotNull(ctype.Parameters.FirstOrDefault(p => p.Name == "boundary"));
        Assert.Contains("photo.jpg",   handler.LastRequestBody!, StringComparison.Ordinal);
        Assert.Contains("image/jpeg",  handler.LastRequestBody!, StringComparison.Ordinal);
        Assert.Contains("name=app_key", handler.LastRequestBody!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_PassesAccessToken_AsSysParam()
    {
        var handler = new TestHandler("""{"code":"0"}""");
        var client = NewClient(handler);

        await client.ExecuteAsync(new LazRequest("/orders/get"), accessToken: "tok-xyz");

        Assert.Contains("access_token=tok-xyz", handler.LastRequestBody!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_Timestamp_IsCoercedToUtcMilliseconds()
    {
        var handler = new TestHandler("""{"code":"0"}""");
        var client = NewClient(handler);

        // Local 2026-01-01 12:00:00 — should be converted to UTC ms.
        var local = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Local);
        var expectedUtcMs = new DateTimeOffset(local.ToUniversalTime(), TimeSpan.Zero).ToUnixTimeMilliseconds();

        await client.ExecuteAsync(new LazRequest("/orders/get") { HttpMethod = Constants.METHOD_GET }, timestamp: local);

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains($"timestamp={expectedUtcMs}", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_RequiresApiName()
    {
        var handler = new TestHandler("""{}""");
        var client = NewClient(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.ExecuteAsync(new LazRequest()));
    }

    [Fact]
    public async Task ExecuteAsync_RespectsCustomServerUrl()
    {
        var handler = new TestHandler("""{"code":"0"}""");
        var client = NewClient(handler, o => o.ServerUrl = UrlConstants.API_GATEWAY_URL_TH);

        await client.ExecuteAsync(new LazRequest("/orders/get") { HttpMethod = Constants.METHOD_GET });

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_TH + "/orders/get?",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }
}
