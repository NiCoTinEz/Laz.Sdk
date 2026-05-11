using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class OrdersServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string SuccessBody = """
    {
      "code":"0",
      "data":{
        "document":{
          "file":"PHN0eWxlPnRlRrU3VRbUNDJyAvPjwvcD4K",
          "mime_type":"text/html",
          "document_type":"shippingLabel"
        }
      },
      "request_id":"0ba2887315178178017221014"
    }
    """;

    [Fact]
    public async Task GetDocumentAsync_ParsesOfficialSample()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        var response = await client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest
            {
                DocType = OrderDocumentType.ShippingLabel,
                OrderItemIds = new[] { 279709L, 279709L },
            },
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("0ba2887315178178017221014", response.RequestId);
        Assert.NotNull(response.Data?.Document);
        Assert.Equal("PHN0eWxlPnRlRrU3VRbUNDJyAvPjwvcD4K", response.Data!.Document!.File);
        Assert.Equal("text/html",     response.Data.Document.MimeType);
        Assert.Equal("shippingLabel", response.Data.Document.DocumentType);
    }

    [Fact]
    public async Task GetDocumentAsync_DecodesFileBytes_FromValidBase64()
    {
        var payload = System.Text.Encoding.UTF8.GetBytes("hello world");
        var base64 = Convert.ToBase64String(payload);
        var body = "{\"code\":\"0\",\"data\":{\"document\":{\"file\":\"" + base64
                 + "\",\"mime_type\":\"text/plain\",\"document_type\":\"invoice\"}},\"request_id\":\"r\"}";
        var client = NewClient(new TestHandler(body));

        var response = await client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = OrderDocumentType.Invoice, OrderItemIds = new[] { 1L } },
            "tok");

        Assert.Equal(payload, response.Data!.Document!.GetFileBytes());
    }

    [Fact]
    public void GetFileBytes_ReturnsEmpty_WhenFileMissing()
    {
        var doc = new OrderDocument { MimeType = "x", DocumentType = "y" };
        Assert.Empty(doc.GetFileBytes());
    }

    [Fact]
    public async Task GetDocumentAsync_BuildsGetUrl_WithExpectedQuery()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest
            {
                DocType = OrderDocumentType.ShippingLabel,
                OrderItemIds = new[] { 279709L, 279710L },
            },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/order/document/get?", url, StringComparison.Ordinal);
        Assert.Contains("doc_type=shippingLabel",                                   url, StringComparison.Ordinal);
        Assert.Contains("order_item_ids=" + Uri.EscapeDataString("[279709,279710]"), url, StringComparison.Ordinal);
        Assert.Contains("access_token=tok",                                          url, StringComparison.Ordinal);
        Assert.Contains("sign=",                                                     url, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(OrderDocumentType.Invoice,         "doc_type=invoice")]
    [InlineData(OrderDocumentType.ShippingLabel,   "doc_type=shippingLabel")]
    [InlineData(OrderDocumentType.CarrierManifest, "doc_type=carrierManifest")]
    public async Task GetDocumentAsync_MapsDocTypeToWireValue(OrderDocumentType docType, string expectedFragment)
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = docType, OrderItemIds = new[] { 1L } },
            "tok");

        Assert.Contains(expectedFragment, handler.LastRequest!.RequestUri!.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetDocumentAsync_FormatsSingleIdAsArray()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = OrderDocumentType.Invoice, OrderItemIds = new[] { 42L } },
            "tok");

        Assert.Contains("order_item_ids=" + Uri.EscapeDataString("[42]"),
            handler.LastRequest!.RequestUri!.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetDocumentAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"32","type":"ISV","message":"E032: Document type \"foo\" is not valid","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = OrderDocumentType.Invoice, OrderItemIds = new[] { 1L } },
            "tok"));

        Assert.Equal("32", ex.ErrorCode);
        Assert.Contains("Document type", ex.ErrorMsg, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetDocumentAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = OrderDocumentType.Invoice, OrderItemIds = new[] { 1L } },
            ""));
    }

    [Fact]
    public async Task GetDocumentAsync_RejectsEmptyOrderItemIds()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = OrderDocumentType.Invoice, OrderItemIds = Array.Empty<long>() },
            "tok"));
    }

    [Fact]
    public async Task GetDocumentAsync_RegionalGateway_RespectsServerUrl()
    {
        var handler = new TestHandler(SuccessBody);
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as", ServerUrl = UrlConstants.API_GATEWAY_URL_TH };
        var http = new HttpClient(handler);
        var client = new LazClient(http, opts);

        await client.Orders.GetDocumentAsync(
            new GetOrderDocumentRequest { DocType = OrderDocumentType.Invoice, OrderItemIds = new[] { 1L } },
            "tok");

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_TH + "/order/document/get?",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }
}
