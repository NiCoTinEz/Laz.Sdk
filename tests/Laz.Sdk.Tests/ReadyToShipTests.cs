using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class ReadyToShipTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """
    {
      "code":"0",
      "data":{
        "order_items":[
          {"order_item_id":"123456","purchase_order_id":"456789","purchase_order_number":"ABC-123456"}
        ]
      },
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task ReadyToShipAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Orders.ReadyToShipAsync(
            new ReadyToShipRequest
            {
                OrderItemIds     = new[] { 1832590L, 1832592L },
                ShipmentProvider = "Aramax",
                TrackingNumber   = "12345678",
            },
            "tok");

        var item = Assert.Single(response.Data!.OrderItems!);
        Assert.Equal("123456",     item.OrderItemId);
        Assert.Equal("456789",     item.PurchaseOrderId);
        Assert.Equal("ABC-123456", item.PurchaseOrderNumber);
    }

    [Fact]
    public async Task ReadyToShipAsync_PostsAllParams()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.ReadyToShipAsync(
            new ReadyToShipRequest
            {
                OrderItemIds     = new[] { 1832590L, 1832592L },
                ShipmentProvider = "Aramax",
                TrackingNumber   = "12345678",
            },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("delivery_type=dropship",                                           body, StringComparison.Ordinal);
        Assert.Contains("order_item_ids=" + Uri.EscapeDataString("[1832590,1832592]"),      body, StringComparison.Ordinal);
        Assert.Contains("shipment_provider=Aramax",                                          body, StringComparison.Ordinal);
        Assert.Contains("tracking_number=12345678",                                          body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ReadyToShipAsync_RejectsMissingTrackingNumber()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Orders.ReadyToShipAsync(
                new ReadyToShipRequest { OrderItemIds = new[] { 1L }, ShipmentProvider = "X", TrackingNumber = "" },
                "tok"));
    }

    [Fact]
    public async Task ReadyToShipAsync_RejectsMissingShipmentProvider()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Orders.ReadyToShipAsync(
                new ReadyToShipRequest { OrderItemIds = new[] { 1L }, ShipmentProvider = "", TrackingNumber = "12" },
                "tok"));
    }

    [Fact]
    public async Task ReadyToShipAsync_Throws_OnError()
    {
        const string ErrorBody = """{"code":"63","type":"ISV","message":"E063: tracking code used","request_id":"r"}""";
        var client = NewClient(new TestHandler(ErrorBody));
        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Orders.ReadyToShipAsync(
                new ReadyToShipRequest { OrderItemIds = new[] { 1L }, ShipmentProvider = "X", TrackingNumber = "T" },
                "tok"));
        Assert.Equal("63", ex.ErrorCode);
    }
}
