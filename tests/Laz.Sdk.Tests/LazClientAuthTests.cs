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

        await client.Auth.CreateAccessTokenAsync("oauth-code-xyz");

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

        var token = await client.Auth.CreateAccessTokenAsync("c");

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

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Auth.CreateAccessTokenAsync("bad"));
        Assert.Equal("IllegalAccessToken", ex.ErrorCode);
        Assert.Equal("invalid code",       ex.ErrorMsg);
    }

    [Fact]
    public async Task CreateAccessTokenAsync_RejectsEmptyCode()
    {
        var client = NewClient(new TestHandler("""{}"""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Auth.CreateAccessTokenAsync(""));
    }

    [Fact]
    public async Task RefreshAccessTokenAsync_HitsRefreshEndpoint_WithRefreshToken()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Auth.RefreshAccessTokenAsync("ref-existing");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_AUTHORIZATION_URL + "/auth/token/refresh?", url, StringComparison.Ordinal);
        Assert.Contains("refresh_token=ref-existing", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task RefreshAccessTokenAsync_RejectsEmptyToken()
    {
        var client = NewClient(new TestHandler("""{}"""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Auth.RefreshAccessTokenAsync(""));
    }

    [Fact]
    public async Task CreateAccessTokenAsync_ParsesOfficialDocSample_WithStringExpiresIn()
    {
        // Verbatim sample from https://open.lazada.com/apps/doc/api?path=/auth/token/create
        // Note: expires_in and refresh_expires_in arrive as JSON strings, not numbers.
        const string OfficialSample = """
        {
          "access_token": "50000601c30atpedfgu3LVvik87Ixlsvle3mSoB7701ceb156fPunYZ43GBg",
          "country": "sg",
          "refresh_token": "500016000300bwa2WteaQyfwBMnPxurcA0mXGhQdTt18356663CfcDTYpWoi",
          "account_id": "7063844",
          "code": "0",
          "account_platform": "seller_center",
          "refresh_expires_in": "60",
          "country_user_info": [
            {
              "country": "SG",
              "user_id": "1001",
              "seller_id": "1001",
              "short_code": "SG1001"
            }
          ],
          "expires_in": "10",
          "request_id": "0ba2887315178178017221014",
          "account": "xxx@126.com"
        }
        """;

        var client = NewClient(new TestHandler(OfficialSample));

        var token = await client.Auth.CreateAccessTokenAsync("0_100132_2DL4DV3jcU1UOT7WGI1A4rY91");

        Assert.Equal("50000601c30atpedfgu3LVvik87Ixlsvle3mSoB7701ceb156fPunYZ43GBg", token.AccessToken);
        Assert.Equal("500016000300bwa2WteaQyfwBMnPxurcA0mXGhQdTt18356663CfcDTYpWoi", token.RefreshToken);
        Assert.Equal(10L, token.ExpiresIn);
        Assert.Equal(60L, token.RefreshExpiresIn);
        Assert.Equal("sg",            token.Country);
        Assert.Equal("seller_center", token.AccountPlatform);
        Assert.Equal("xxx@126.com",   token.Account);
        Assert.Equal("7063844",       token.AccountId);
        Assert.Equal("0ba2887315178178017221014", token.RequestId);
        Assert.Equal("0",             token.Code);

        Assert.NotNull(token.CountryUserInfo);
        var sg = Assert.Single(token.CountryUserInfo!);
        Assert.Equal("SG",     sg.Country);
        Assert.Equal("1001",   sg.UserId);
        Assert.Equal("1001",   sg.SellerId);
        Assert.Equal("SG1001", sg.ShortCode);
    }

    [Fact]
    public async Task AuthEndpoints_DoNotSendAccessToken_AsParam()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Auth.CreateAccessTokenAsync("code-xyz");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.DoesNotContain("access_token=", url, StringComparison.Ordinal);
    }
}
