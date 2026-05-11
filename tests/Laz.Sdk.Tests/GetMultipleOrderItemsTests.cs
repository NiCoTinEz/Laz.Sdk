using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class GetMultipleOrderItemsTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    // Sample distilled from official /orders/items/get response. Top-level data is array of groups.
    // Note new field 'show_gift_wrapping_tag' (underscored) vs /order/items/get's 'show_giftwrapping_tag'.
    private const string SuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "order_number":"300029225",
          "order_id":"32793",
          "order_items":[
            {
              "order_item_id":"100827",
              "order_id":"32793",
              "sku":"BRSCR#06",
              "shop_sku":"BE494HLSSSDTSGAMZ-39888",
              "name":"Bean Rester Crest Brown",
              "show_gift_wrapping_tag":"True",
              "show_personalization_tag":"True",
              "is_cancel_pending":"true",
              "is_digital":"0",
              "is_fbl":"0",
              "is_reroute":"0",
              "status":"delivered",
              "paid_price":"89.10",
              "currency":"SGD",
              "created_at":"1413786247000",
              "updated_at":"1414548487000",
              "tracking_code":"2014038590005",
              "shipment_provider":"TA-Q-BIN",
              "extra_attributes":null
            }
          ]
        }
      ],
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task GetMultipleOrderItemsAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Orders.GetMultipleOrderItemsAsync(
            new GetMultipleOrderItemsRequest { OrderIds = new[] { 42922L, 32793L } },
            "tok");

        var group = Assert.Single(response.Data!);
        Assert.Equal("32793",     group.OrderId);
        Assert.Equal("300029225", group.OrderNumber);

        var item = Assert.Single(group.OrderItems!);
        Assert.Equal("100827",  item.OrderItemId);
        Assert.Equal("BRSCR#06", item.Sku);
        Assert.True(item.ShowGiftWrappingTag);            // resolves via the underscored property
        Assert.False(item.ShowGiftWrappingTagCompact);    // not set in this payload
        Assert.True(item.ShowGiftWrappingTagUnderscored); // wired here
        Assert.Equal("TA-Q-BIN", item.ShipmentProvider);
    }

    [Fact]
    public async Task GetMultipleOrderItemsAsync_BuildsUrl_WithJsonArrayIds()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetMultipleOrderItemsAsync(
            new GetMultipleOrderItemsRequest { OrderIds = new[] { 42922L, 32793L } },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/orders/items/get?", url, StringComparison.Ordinal);
        Assert.Contains("order_ids=" + Uri.EscapeDataString("[42922,32793]"), url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetMultipleOrderItemsAsync_RejectsEmptyIds()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Orders.GetMultipleOrderItemsAsync(
                new GetMultipleOrderItemsRequest { OrderIds = Array.Empty<long>() },
                "tok"));
    }

    [Fact]
    public async Task GetMultipleOrderItemsAsync_RejectsMoreThan50Ids()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        var ids = Enumerable.Range(1, 51).Select(i => (long)i).ToArray();
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Orders.GetMultipleOrderItemsAsync(
                new GetMultipleOrderItemsRequest { OrderIds = ids },
                "tok"));
    }

    [Fact]
    public async Task GetMultipleOrderItemsAsync_Throws_OnError()
    {
        const string ErrorBody = """{"code":"38","type":"ISV","message":"E038: Too many orders","request_id":"r"}""";
        var client = NewClient(new TestHandler(ErrorBody));
        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Orders.GetMultipleOrderItemsAsync(
                new GetMultipleOrderItemsRequest { OrderIds = new[] { 1L } },
                "tok"));
        Assert.Equal("38", ex.ErrorCode);
    }
}
