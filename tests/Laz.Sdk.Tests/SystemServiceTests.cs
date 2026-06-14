using Laz.Sdk.Models.System;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class SystemServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string TokenWithOpenIdSuccessBody = """
    {
      "access_token":"5000abc",
      "refresh_token":"5000def",
      "expires_in":"3600",
      "refresh_expires_in":"2592000",
      "country":"sg",
      "account_platform":"seller_center",
      "account":"seller@example.com",
      "account_id":"12345",
      "open_id":"open-xyz-789",
      "country_user_info":[
        {"country":"sg","seller_id":"1001","short_code":"SC001"}
      ],
      "code":"0",
      "request_id":"req-oid-001"
    }
    """;

    private const string DataMopFormatSuccessBody = """
    {
      "code":"0",
      "data":{
        "format":"xml"
      },
      "request_id":"req-mop-001"
    }
    """;

    // ─── CreateAccessTokenWithOpenIdAsync ───────────────────────────────

    [Fact]
    public async Task CreateAccessTokenWithOpenIdAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(TokenWithOpenIdSuccessBody);
        var client = NewClient(handler);

        var token = await client.System.CreateAccessTokenWithOpenIdAsync("auth-code-123");

        Assert.Equal("5000abc", token.AccessToken);
        Assert.Equal("5000def", token.RefreshToken);
        Assert.Equal(3600, token.ExpiresIn);
        Assert.Equal(2592000, token.RefreshExpiresIn);
        Assert.Equal("sg", token.Country);
        Assert.Equal("open-xyz-789", token.OpenId);
        Assert.NotNull(token.CountryUserInfo);
        var info = Assert.Single(token.CountryUserInfo!);
        Assert.Equal("sg", info.Country);
        Assert.Equal("1001", info.SellerId);
    }

    [Fact]
    public async Task CreateAccessTokenWithOpenIdAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(TokenWithOpenIdSuccessBody);
        var client = NewClient(handler);

        await client.System.CreateAccessTokenWithOpenIdAsync("auth-code-123");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_AUTHORIZATION_URL + "/auth/token/createWithOpenId?", url, StringComparison.Ordinal);
        Assert.Contains("code=auth-code-123", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task CreateAccessTokenWithOpenIdAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"400","type":"ISV","message":"Invalid code","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.System.CreateAccessTokenWithOpenIdAsync("bad-code"));
        Assert.Equal("400", ex.ErrorCode);
    }

    [Fact]
    public async Task CreateAccessTokenWithOpenIdAsync_RejectsEmptyCode()
    {
        var client = NewClient(new TestHandler(TokenWithOpenIdSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.System.CreateAccessTokenWithOpenIdAsync(""));
    }

    // ─── GetDataMopFormatAsync ──────────────────────────────────────────

    [Fact]
    public async Task GetDataMopFormatAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(DataMopFormatSuccessBody);
        var client = NewClient(handler);

        var response = await client.System.GetDataMopFormatAsync(accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("req-mop-001", response.RequestId);
        Assert.NotNull(response.Data);
        Assert.Equal("xml", response.Data!.Format);
    }

    [Fact]
    public async Task GetDataMopFormatAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(DataMopFormatSuccessBody);
        var client = NewClient(handler);

        await client.System.GetDataMopFormatAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/data/mop/format/get?", url, StringComparison.Ordinal);
        Assert.Contains("access_token=tok", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetDataMopFormatAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"500","type":"ISV","message":"Server error","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.System.GetDataMopFormatAsync("tok"));
        Assert.Equal("500", ex.ErrorCode);
    }

    [Fact]
    public async Task GetDataMopFormatAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(DataMopFormatSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.System.GetDataMopFormatAsync(""));
    }
}
