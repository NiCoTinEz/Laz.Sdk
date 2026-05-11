using Laz.Sdk.Models;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class LazClientAuthTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string SuccessBody = """
    {
      "access_token":"acc-123",
      "refresh_token":"ref-456",
      "expires_in":604800,
      "refresh_expires_in":2592000,
      "country":"sg",
      "account_platform":"seller_center",
      "account":"seller@example.com",
      "account_id":"100000123",
      "country_user_info":[
        {"country":"sg","user_id":"u-sg","seller_id":"s-sg","short_code":"SG"},
        {"country":"my","user_id":"u-my","seller_id":"s-my","short_code":"MY"}
      ],
      "request_id":"req-abc"
    }
    """;

    [Fact]
    public async Task CreateAccessTokenAsync_HitsAuthGateway_Get_WithCode()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.CreateAccessTokenAsync("oauth-code-xyz");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_AUTHORIZATION_URL + "/auth/token/create?", url, StringComparison.Ordinal);
        Assert.Contains("code=oauth-code-xyz", url, StringComparison.Ordinal);
        Assert.Contains("app_key=ak", url, StringComparison.Ordinal);
        Assert.Contains("sign=",      url, StringComparison.Ordinal);
        Assert.Contains("timestamp=", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task CreateAccessTokenAsync_ReturnsTypedToken()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        var token = await client.CreateAccessTokenAsync("c");

        Assert.Equal("acc-123",            token.AccessToken);
        Assert.Equal("ref-456",            token.RefreshToken);
        Assert.Equal(604800L,              token.ExpiresIn);
        Assert.Equal(2592000L,             token.RefreshExpiresIn);
        Assert.Equal("sg",                 token.Country);
        Assert.Equal("seller_center",      token.AccountPlatform);
        Assert.Equal("seller@example.com", token.Account);
        Assert.Equal("100000123",          token.AccountId);
        Assert.Equal("req-abc",            token.RequestId);

        Assert.NotNull(token.CountryUserInfo);
        Assert.Equal(2, token.CountryUserInfo!.Count);
        Assert.Equal("SG", token.CountryUserInfo[0].ShortCode);
        Assert.Equal("MY", token.CountryUserInfo[1].ShortCode);
    }

    [Fact]
    public async Task CreateAccessTokenAsync_ThrowsLazException_OnErrorBody()
    {
        var errorBody = """
        {"code":"IllegalAccessToken","type":"ISV","message":"invalid code","request_id":"req-err"}
        """;
        var handler = new TestHandler(errorBody);
        var client = NewClient(handler);

        var ex = await Assert.ThrowsAsync<LazException>(() => client.CreateAccessTokenAsync("bad"));
        Assert.Equal("IllegalAccessToken", ex.ErrorCode);
        Assert.Equal("invalid code",       ex.ErrorMsg);
    }

    [Fact]
    public async Task CreateAccessTokenAsync_RejectsEmptyCode()
    {
        var client = NewClient(new TestHandler("""{}"""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CreateAccessTokenAsync(""));
    }

    [Fact]
    public async Task RefreshAccessTokenAsync_HitsRefreshEndpoint_WithRefreshToken()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.RefreshAccessTokenAsync("ref-existing");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_AUTHORIZATION_URL + "/auth/token/refresh?", url, StringComparison.Ordinal);
        Assert.Contains("refresh_token=ref-existing", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task RefreshAccessTokenAsync_RejectsEmptyToken()
    {
        var client = NewClient(new TestHandler("""{}"""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.RefreshAccessTokenAsync(""));
    }

    [Fact]
    public async Task AuthEndpoints_DoNotSendAccessToken_AsParam()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.CreateAccessTokenAsync("code-xyz");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.DoesNotContain("access_token=", url, StringComparison.Ordinal);
    }
}
