using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class WithCredentialsTests
{
    private static (LazClient, TestHandler) NewClient(string body = """{"code":"0"}""")
    {
        var handler = new TestHandler(body);
        var opts = new LazClientOptions { AppKey = "default-key", AppSecret = "default-secret" };
        var http = new HttpClient(handler);
        return (new LazClient(http, opts), handler);
    }

    [Fact]
    public void WithCredentials_RequiresArgument()
    {
        var (client, _) = NewClient();
        Assert.Throws<ArgumentNullException>(() => client.WithCredentials(null!));
    }

    [Fact]
    public void WithCredentials_ReturnsDistinctClient()
    {
        var (client, _) = NewClient();
        var scoped = client.WithCredentials(new LazCredentials("k", "s"));
        Assert.NotSame(client, scoped);
    }

    [Fact]
    public async Task WithCredentials_OverridesAppKey_OnExecuteAsync()
    {
        var (client, handler) = NewClient();
        var scoped = client.WithCredentials(new LazCredentials("tenant-A", "secret-A"));

        await scoped.ExecuteAsync(new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET });

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("app_key=tenant-A", url, StringComparison.Ordinal);
        Assert.DoesNotContain("app_key=default-key", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task WithCredentials_ChangesSignature_VsDefault()
    {
        // Same request, different secrets => different signatures.
        // Fresh handler builds a new response per call so it survives multiple sends.
        var handler = new TestHandler(_ => Task.FromResult(
            new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("""{"code":"0"}""", System.Text.Encoding.UTF8, "application/json"),
            }));
        var opts = new LazClientOptions { AppKey = "default-key", AppSecret = "default-secret" };
        var client = new LazClient(new HttpClient(handler), opts);
        var fixedTs = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        await client.ExecuteAsync(new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET }, timestamp: fixedTs);
        var signDefault = ExtractParam(handler.LastRequest!.RequestUri!.ToString(), "sign");

        var scoped = client.WithCredentials(new LazCredentials("default-key", "DIFFERENT-secret"));
        await scoped.ExecuteAsync(new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET }, timestamp: fixedTs);
        var signScoped = ExtractParam(handler.LastRequest!.RequestUri!.ToString(), "sign");

        Assert.NotEqual(signDefault, signScoped);
    }

    [Fact]
    public async Task WithCredentials_OverridesServerUrl_WhenProvided()
    {
        var (client, handler) = NewClient();
        var scoped = client.WithCredentials(new LazCredentials("k", "s", UrlConstants.API_GATEWAY_URL_VN));

        await scoped.ExecuteAsync(new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET });

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_VN + "/x/y?",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task WithCredentials_FallsBackToOptionsServerUrl_WhenNull()
    {
        var (client, handler) = NewClient();
        var scoped = client.WithCredentials(new LazCredentials("k", "s", ServerUrl: null));

        await scoped.ExecuteAsync(new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET });

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_SG + "/x/y?",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task WithCredentials_PropagatesToTypedService_Orders()
    {
        const string SuccessBody = """{"code":"0","data":{"document":{"file":"","mime_type":"text/html","document_type":"shippingLabel"}}}""";
        var (client, handler) = NewClient(SuccessBody);
        var scoped = client.WithCredentials(new LazCredentials("tenant-B", "secret-B", UrlConstants.API_GATEWAY_URL_TH));

        await scoped.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = OrderDocumentType.ShippingLabel, OrderItemIds = new[] { 1L } },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_TH + "/order/document/get?", url, StringComparison.Ordinal);
        Assert.Contains("app_key=tenant-B", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_WithoutCredentials_ThrowsInvalidOperation()
    {
        // No AppKey/AppSecret on options, no WithCredentials => clear error.
        var handler = new TestHandler("""{"code":"0"}""");
        var opts = new LazClientOptions { ServerUrl = UrlConstants.API_GATEWAY_URL_SG };
        var client = new LazClient(new HttpClient(handler), opts);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            client.ExecuteAsync(new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET }));
        Assert.Contains("AppKey", ex.Message, StringComparison.Ordinal);
        Assert.Contains("AppSecret", ex.Message, StringComparison.Ordinal);
        Assert.Contains("WithCredentials", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_WithCredentialsOnly_Works_WhenOptionsBlank()
    {
        // Multi-tenant happy path: options have no creds, WithCredentials supplies them.
        var handler = new TestHandler("""{"code":"0"}""");
        var opts = new LazClientOptions { ServerUrl = UrlConstants.API_GATEWAY_URL_SG };
        var client = new LazClient(new HttpClient(handler), opts);

        var scoped = client.WithCredentials(new LazCredentials("tenant-key", "tenant-secret"));
        await scoped.ExecuteAsync(new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET });

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("app_key=tenant-key", url, StringComparison.Ordinal);
        Assert.Contains("sign=",              url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task WithCredentials_DoesNotAffectOriginalClient()
    {
        var (client, handler) = NewClient();
        _ = client.WithCredentials(new LazCredentials("tenant-X", "secret-X"));

        await client.ExecuteAsync(new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET });

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("app_key=default-key", url, StringComparison.Ordinal);
        Assert.DoesNotContain("app_key=tenant-X", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_InlineCredentials_Override()
    {
        var (client, handler) = NewClient();

        await client.ExecuteAsync(
            new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET },
            credentials: new LazCredentials("inline-key", "inline-secret"));

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("app_key=inline-key", url, StringComparison.Ordinal);
        Assert.DoesNotContain("app_key=default-key", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_InlineCredentials_BeatScopedAndOptions()
    {
        var (client, handler) = NewClient();
        var scoped = client.WithCredentials(new LazCredentials("scoped-key", "scoped-secret"));

        await scoped.ExecuteAsync(
            new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET },
            credentials: new LazCredentials("inline-key", "inline-secret"));

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("app_key=inline-key", url, StringComparison.Ordinal);
        Assert.DoesNotContain("app_key=scoped-key",  url, StringComparison.Ordinal);
        Assert.DoesNotContain("app_key=default-key", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_InlineCredentials_OverrideServerUrl()
    {
        var (client, handler) = NewClient();

        await client.ExecuteAsync(
            new LazRequest("/x/y") { HttpMethod = Constants.METHOD_GET },
            credentials: new LazCredentials("k", "s", UrlConstants.API_GATEWAY_URL_ID));

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_ID + "/x/y?",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task OrdersGetDocumentAsync_AcceptsInlineCredentials()
    {
        const string SuccessBody = """{"code":"0","data":{"document":{"file":"","mime_type":"text/html","document_type":"shippingLabel"}}}""";
        var (client, handler) = NewClient(SuccessBody);

        await client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = OrderDocumentType.ShippingLabel, OrderItemIds = new[] { 1L } },
            accessToken: "tok",
            credentials: new LazCredentials("tenant-K", "tenant-S", UrlConstants.API_GATEWAY_URL_PH));

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_PH + "/order/document/get?", url, StringComparison.Ordinal);
        Assert.Contains("app_key=tenant-K", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task AuthCreateAccessTokenAsync_AcceptsInlineCredentials()
    {
        const string SuccessBody = """
        {"access_token":"a","refresh_token":"r","expires_in":"10","refresh_expires_in":"60","code":"0","request_id":"r"}
        """;
        var (client, handler) = NewClient(SuccessBody);

        await client.Auth.CreateAccessTokenAsync(
            "code-xyz",
            credentials: new LazCredentials("alt-key", "alt-secret"));

        // Auth gateway is fixed; only app_key should reflect override.
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_AUTHORIZATION_URL + "/auth/token/create?", url, StringComparison.Ordinal);
        Assert.Contains("app_key=alt-key", url, StringComparison.Ordinal);
    }

    private static string ExtractParam(string url, string name)
    {
        var q = url[(url.IndexOf('?', StringComparison.Ordinal) + 1)..];
        foreach (var pair in q.Split('&'))
        {
            var eq = pair.IndexOf('=', StringComparison.Ordinal);
            if (eq > 0 && pair[..eq] == name)
            {
                return Uri.UnescapeDataString(pair[(eq + 1)..]);
            }
        }
        return "";
    }
}
