using Laz.Sdk.Models.Fulfillment;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class DbsFulfillmentTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SofBody = """
    {
      "result":{
        "error_msg":"operation not support fot nonDBS order!",
        "data":{"packages":[{"msg":"x","item_err_code":"700009","package_id":"FP038521002","retry":"false"}]},
        "success":"false",
        "error_code":"700009"
      },
      "code":"0",
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task ConfirmDbsDeliveryAsync_PostsDbsDeliveryReq()
    {
        var handler = new TestHandler(SofBody);
        var client = NewClient(handler);

        var response = await client.Fulfillment.ConfirmDbsDeliveryAsync(
            new ConfirmDbsDeliveryRequest { Packages = new[] { new AwbPackage { PackageId = "FP234234" } } },
            "tok");

        Assert.False(response.Result!.Success);
        Assert.Equal("FP038521002", response.Result.Data!.Packages![0].PackageId);
        Assert.Contains("dbsDeliveryReq=", handler.LastRequestBody!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task FailedDbsDeliveryAsync_PostsDbsFailedDeliveryReq()
    {
        var handler = new TestHandler(SofBody);
        var client = NewClient(handler);

        await client.Fulfillment.FailedDbsDeliveryAsync(
            new FailedDbsDeliveryRequest { Packages = new[] { new AwbPackage { PackageId = "FP1" } } },
            "tok");

        Assert.Contains("dbsFailedDeliveryReq=", handler.LastRequestBody!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task PackageStatusUpdateForDbsAsync_PostsAllParams()
    {
        const string SuccessBody = """
        {
          "code":"0",
          "success":"true",
          "module":{"result":"true"},
          "errorCode":{"displayMessage":"error msesage"},
          "request_id":"r"
        }
        """;
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        var response = await client.Fulfillment.PackageStatusUpdateForDbsAsync(
            new PackageStatusUpdateForDbsRequest
            {
                TrackingNumber = "SOF123456",
                CarrierCode    = "SF",
                PackageId      = "FP043412484186001",
                TrackInfo      = new DbsTrackInfo
                {
                    LatestStatus = new DbsTrackLatestStatus { Status = "status", SubStatus = "subStatus", SubStatusDesc = "subStatusDesc" },
                    LatestEvent  = new DbsTrackLatestEvent { Stage = "stage", EventTime = 1723012167919L, Description = "description", Location = "location" },
                },
            },
            "tok");

        Assert.True(response.Success);
        Assert.True(response.Module!.Result);
        Assert.Equal("error msesage", response.ErrorCode!.DisplayMessage);

        var body = handler.LastRequestBody!;
        Assert.Contains("trackingNumber=SOF123456", body, StringComparison.Ordinal);
        Assert.Contains("source=OPENAPI",            body, StringComparison.Ordinal);
        Assert.Contains("carrierCode=SF",            body, StringComparison.Ordinal);
        Assert.Contains("tag=FP043412484186001",     body, StringComparison.Ordinal);
        Assert.Contains("trackInfo=",                body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task DeliverDigitalAsync_BuildsDigitalDeliveryReq()
    {
        const string SuccessBody = """
        {
          "result":{
            "data":{"orders":[{"order_item_list":[{"msg":"X","order_item_id":"526170322294184","item_err_code":"700020","retry":"false"}],"order_id":"526170322194184"}]},
            "success":"true",
            "errorCode":"700019",
            "errorMsg":"order can't be null"
          },
          "code":"0",
          "request_id":"r"
        }
        """;
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        var response = await client.Fulfillment.DeliverDigitalAsync(
            new DeliverDigitalRequest
            {
                Orders = new[]
                {
                    new DigitalDeliveryOrder
                    {
                        OrderId       = "2342342",
                        OrderItemList = new IReadOnlyList<long>[] { new[] { 1L, 2L } },
                    },
                },
            },
            "tok");

        var order = Assert.Single(response.Result!.Data!.Orders!);
        Assert.Equal("526170322194184", order.OrderId);
        Assert.Equal("526170322294184", order.OrderItemList![0].OrderItemId);

        var body = handler.LastRequestBody!;
        var i = body.IndexOf("digitalDeliveryReq=", StringComparison.Ordinal) + "digitalDeliveryReq=".Length;
        var raw = body[i..];
        var amp = raw.IndexOf('&');
        if (amp >= 0) raw = raw[..amp];
        var decoded = Uri.UnescapeDataString(raw);

        Assert.Contains("\"order_id\":\"2342342\"", decoded, StringComparison.Ordinal);
        Assert.Contains("[1,2]",                    decoded, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ConfirmDbsDeliveryAsync_RejectsEmptyPackages()
    {
        var client = NewClient(new TestHandler(SofBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Fulfillment.ConfirmDbsDeliveryAsync(
                new ConfirmDbsDeliveryRequest { Packages = Array.Empty<AwbPackage>() },
                "tok"));
    }
}
