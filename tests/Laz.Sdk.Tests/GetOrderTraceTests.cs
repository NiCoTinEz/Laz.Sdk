using Laz.Sdk.Models.Logistics;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class GetOrderTraceTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """
    {
      "result":{
        "not_success":"false",
        "success":"true",
        "module":[
          {
            "warehouse_detail_info":null,
            "ofc_order_id":null,
            "package_detail_info_list":[
              {
                "order_line_info_list":null,
                "ofc_package_id":"FP032211046428116",
                "tracking_number":"NLXSG20300914",
                "logistic_detail_info_list":[
                  {
                    "package_location_name":null,
                    "status_code":"1200",
                    "proof_images":[],
                    "detail_type":"ready_to",
                    "event_date":null,
                    "receive_time":"0",
                    "icon":null,
                    "description":"Your parcel has been packed and ready to be handed over to our shipping provider.",
                    "title":"Packed by seller / warehouse",
                    "event_time":"1625987646597"
                  }
                ]
              }
            ]
          }
        ],
        "error_code":{"displayMessage":null},
        "repeated":"false",
        "retry":"false"
      },
      "code":"0",
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task GetOrderTraceAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Logistics.GetOrderTraceAsync(
            new GetOrderTraceRequest { OrderId = "56150613585762", Locale = "en" },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.True(response.Result!.Success);
        Assert.False(response.Result.Repeated);

        var module = Assert.Single(response.Result.Module!);
        var pkg = Assert.Single(module.PackageDetailInfoList!);
        Assert.Equal("FP032211046428116", pkg.OfcPackageId);
        Assert.Equal("NLXSG20300914",     pkg.TrackingNumber);

        var ev = Assert.Single(pkg.LogisticDetailInfoList!);
        Assert.Equal(1200,                ev.StatusCode);
        Assert.Equal("ready_to",          ev.DetailType);
        Assert.Equal("Packed by seller / warehouse", ev.Title);
        Assert.Equal(1625987646597L,      ev.EventTime);
    }

    [Fact]
    public async Task GetOrderTraceAsync_BuildsUrl_WithAllParams()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Logistics.GetOrderTraceAsync(
            new GetOrderTraceRequest
            {
                OrderId          = "56150613585762",
                Locale           = "en",
                OfcPackageIdList = new[] { "FP1", "FP2" },
            },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/logistic/order/trace?", url, StringComparison.Ordinal);
        Assert.Contains("order_id=56150613585762",                              url, StringComparison.Ordinal);
        Assert.Contains("locale=en",                                            url, StringComparison.Ordinal);
        Assert.Contains("ofcPackageIdList=" + Uri.EscapeDataString("[FP1,FP2]"), url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetOrderTraceAsync_DefaultsOfcPackageIdList_ToEmptyArray()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Logistics.GetOrderTraceAsync(
            new GetOrderTraceRequest { OrderId = "X" },
            "tok");

        Assert.Contains("ofcPackageIdList=" + Uri.EscapeDataString("[]"),
            handler.LastRequest!.RequestUri!.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetOrderTraceAsync_RejectsEmptyOrderId()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Logistics.GetOrderTraceAsync(new GetOrderTraceRequest { OrderId = "" }, "tok"));
    }
}
