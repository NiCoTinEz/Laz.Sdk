using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class PackOrderTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """
    {
      "code":"0",
      "data":{
        "order_items":[
          {
            "order_item_id":"123456",
            "purchase_order_id":"567890",
            "purchase_order_number":"ABC-123456",
            "tracking_number":"TRACK-1126935-4306",
            "shipment_provider":"5",
            "package_id":"MPDS-200131783-9800"
          }
        ]
      },
      "request_id":"0ba2887315178178017221014"
    }
    """;

    [Fact]
    public async Task PackAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Orders.PackAsync(
            new PackOrderRequest
            {
                ShippingProvider = "Aramax",
                OrderItemIds     = new[] { 1530553L, 1830236L },
            },
            "tok");

        Assert.Equal("0", response.Code);
        var item = Assert.Single(response.Data!.OrderItems!);
        Assert.Equal("123456",              item.OrderItemId);
        Assert.Equal("567890",              item.PurchaseOrderId);
        Assert.Equal("ABC-123456",          item.PurchaseOrderNumber);
        Assert.Equal("TRACK-1126935-4306",  item.TrackingNumber);
        Assert.Equal("5",                    item.ShipmentProvider);
        Assert.Equal("MPDS-200131783-9800", item.PackageId);
    }

    [Fact]
    public async Task PackAsync_PostsFormBody_WithAllParams()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.PackAsync(
            new PackOrderRequest
            {
                ShippingProvider = "Aramax",
                DeliveryType     = "dropship",
                OrderItemIds     = new[] { 1530553L, 1830236L },
            },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/order/pack", handler.LastRequest.RequestUri!.ToString(), StringComparison.Ordinal);
        Assert.Equal("application/x-www-form-urlencoded", handler.LastRequest.Content!.Headers.ContentType!.MediaType);

        var body = handler.LastRequestBody!;
        Assert.Contains("shipping_provider=Aramax",                                   body, StringComparison.Ordinal);
        Assert.Contains("delivery_type=dropship",                                     body, StringComparison.Ordinal);
        Assert.Contains("order_item_ids=" + Uri.EscapeDataString("[1530553,1830236]"), body, StringComparison.Ordinal);
        Assert.Contains("access_token=tok",                                            body, StringComparison.Ordinal);
        Assert.Contains("sign=",                                                       body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task PackAsync_DefaultsDeliveryType_ToDropship()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.PackAsync(
            new PackOrderRequest { ShippingProvider = "Aramax", OrderItemIds = new[] { 1L } },
            "tok");

        Assert.Contains("delivery_type=dropship", handler.LastRequestBody!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task PackAsync_RejectsEmptyShippingProvider()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Orders.PackAsync(
                new PackOrderRequest { ShippingProvider = "", OrderItemIds = new[] { 1L } },
                "tok"));
    }

    [Fact]
    public async Task PackAsync_RejectsEmptyOrderItemIds()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Orders.PackAsync(
                new PackOrderRequest { ShippingProvider = "Aramax", OrderItemIds = Array.Empty<long>() },
                "tok"));
    }

    [Fact]
    public async Task PackAsync_Throws_OnError()
    {
        const string ErrorBody = """{"code":"25","type":"ISV","message":"E025: Invalid Shipping Provider","request_id":"r"}""";
        var client = NewClient(new TestHandler(ErrorBody));
        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Orders.PackAsync(
                new PackOrderRequest { ShippingProvider = "bogus", OrderItemIds = new[] { 1L } },
                "tok"));
        Assert.Equal("25", ex.ErrorCode);
    }
}
