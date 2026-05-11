using Laz.Sdk.Models.Fulfillment;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class PackV2Tests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """
    {
      "result":{
        "error_msg":"order not found",
        "data":{
          "pack_order_list":[
            {
              "order_item_list":[
                {
                  "order_item_id":"560694402292001",
                  "msg":"success",
                  "item_err_code":"0",
                  "tracking_number":"TH340231JV0W0A",
                  "shipment_provider":"Flash Express",
                  "package_id":"FP022511752246001",
                  "retry":"false"
                }
              ],
              "order_id":"560694402192001"
            }
          ]
        },
        "success":"true",
        "error_code":"700100"
      },
      "code":"0",
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task PackV2Async_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Fulfillment.PackV2Async(
            new PackV2Request
            {
                PackOrderList = new[]
                {
                    new PackOrderInput
                    {
                        OrderId       = "560694402192001",
                        OrderItemList = new IReadOnlyList<long>[] { new[] { 560694402292001L } },
                    },
                },
                ShipmentProviderCode = "FM49",
            },
            "tok");

        Assert.True(response.Result!.Success);
        var group = Assert.Single(response.Result.Data!.PackOrderList!);
        Assert.Equal("560694402192001", group.OrderId);

        var item = Assert.Single(group.OrderItemList!);
        Assert.Equal("560694402292001",   item.OrderItemId);
        Assert.Equal("success",           item.Msg);
        Assert.Equal(0,                   item.ItemErrCode);
        Assert.Equal("TH340231JV0W0A",    item.TrackingNumber);
        Assert.Equal("Flash Express",     item.ShipmentProvider);
        Assert.Equal("FP022511752246001", item.PackageId);
        Assert.False(item.Retry);
    }

    [Fact]
    public async Task PackV2Async_BuildsPackReq_Payload()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Fulfillment.PackV2Async(
            new PackV2Request
            {
                PackOrderList = new[]
                {
                    new PackOrderInput
                    {
                        OrderId       = "23423423",
                        OrderItemList = new IReadOnlyList<long>[] { new[] { 1L, 2L }, new[] { 3L } },
                    },
                },
                ShipmentProviderCode = "FM49",
            },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("packReq=",                                          body, StringComparison.Ordinal);
        var i = body.IndexOf("packReq=", StringComparison.Ordinal) + "packReq=".Length;
        var raw = body[i..];
        var amp = raw.IndexOf('&');
        if (amp >= 0) raw = raw[..amp];
        var decoded = Uri.UnescapeDataString(raw);

        Assert.Contains("\"delivery_type\":\"dropship\"",        decoded, StringComparison.Ordinal);
        Assert.Contains("\"shipment_provider_code\":\"FM49\"",   decoded, StringComparison.Ordinal);
        Assert.Contains("\"shipping_allocate_type\":\"TFS\"",    decoded, StringComparison.Ordinal);
        Assert.Contains("\"order_id\":\"23423423\"",             decoded, StringComparison.Ordinal);
        Assert.Contains("[1,2]",                                 decoded, StringComparison.Ordinal);
        Assert.Contains("[3]",                                   decoded, StringComparison.Ordinal);
    }

    [Fact]
    public async Task PackV2Async_RejectsEmptyPackOrderList()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Fulfillment.PackV2Async(
                new PackV2Request { PackOrderList = Array.Empty<PackOrderInput>(), ShipmentProviderCode = "FM49" },
                "tok"));
    }

    [Fact]
    public async Task PackV2Async_RejectsEmptyShipmentProviderCode()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Fulfillment.PackV2Async(
                new PackV2Request
                {
                    PackOrderList = new[] { new PackOrderInput { OrderId = "1", OrderItemList = new IReadOnlyList<long>[] { new[] { 1L } } } },
                    ShipmentProviderCode = "",
                },
                "tok"));
    }
}
