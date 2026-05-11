using Laz.Sdk.Models.Fulfillment;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class GetShipmentProvidersTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """
    {
      "result":{
        "error_msg":"seller not found",
        "data":{
          "platform_default":"1",
          "shipment_providers":[{"name":"Cainiao","provider_code":"asc_xxx_xxx"}],
          "shipping_allocate_type":"TFS"
        },
        "success":"true",
        "error_code":"70011"
      },
      "code":"0",
      "request_id":"0ba2887315178178017221014"
    }
    """;

    [Fact]
    public async Task GetShipmentProvidersAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Fulfillment.GetShipmentProvidersAsync(
            new GetShipmentProvidersRequest
            {
                Orders = new[]
                {
                    new ShipmentLookupOrder
                    {
                        OrderId = "23423423",
                        OrderItemIds = new IReadOnlyList<long>[] { new[] { 2342342L, 23423L } },
                    },
                },
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Result);
        Assert.True(response.Result!.Success);
        Assert.Equal(1, response.Result.Data!.PlatformDefault);
        Assert.Equal("TFS", response.Result.Data.ShippingAllocateType);
        var sp = Assert.Single(response.Result.Data.ShipmentProviders!);
        Assert.Equal("Cainiao",     sp.Name);
        Assert.Equal("asc_xxx_xxx", sp.ProviderCode);
    }

    [Fact]
    public async Task GetShipmentProvidersAsync_EncodesOrderItemIds_AsStringifiedArrays()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Fulfillment.GetShipmentProvidersAsync(
            new GetShipmentProvidersRequest
            {
                Orders = new[]
                {
                    new ShipmentLookupOrder
                    {
                        OrderId = "23423423",
                        OrderItemIds = new IReadOnlyList<long>[]
                        {
                            new[] { 2342342L, 23423L },
                            new[] { 9999L },
                        },
                    },
                },
            },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/order/shipment/providers/get?", url, StringComparison.Ordinal);

        // The wire form is the entire getShipmentProvidersReq JSON, URL-encoded.
        // Pluck the param back out and assert on the decoded JSON.
        var i = url.IndexOf("getShipmentProvidersReq=", StringComparison.Ordinal);
        Assert.True(i >= 0, $"getShipmentProvidersReq missing in URL: {url}");
        var raw = url[(i + "getShipmentProvidersReq=".Length)..];
        var amp = raw.IndexOf('&');
        if (amp >= 0) raw = raw[..amp];
        var decoded = Uri.UnescapeDataString(raw);

        Assert.Contains("\"order_id\":\"23423423\"",          decoded, StringComparison.Ordinal);
        Assert.Contains("[2342342,23423]",                    decoded, StringComparison.Ordinal);
        Assert.Contains("[9999]",                             decoded, StringComparison.Ordinal);
        Assert.Contains("\"order_item_ids\":[",               decoded, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetShipmentProvidersAsync_RejectsEmptyOrders()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Fulfillment.GetShipmentProvidersAsync(
                new GetShipmentProvidersRequest { Orders = Array.Empty<ShipmentLookupOrder>() },
                "tok"));
    }

    [Fact]
    public async Task GetShipmentProvidersAsync_Throws_OnPlatformError()
    {
        const string ErrorBody = """{"code":"50","type":"ISV","message":"system error","request_id":"r"}""";
        var client = NewClient(new TestHandler(ErrorBody));
        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Fulfillment.GetShipmentProvidersAsync(
                new GetShipmentProvidersRequest
                {
                    Orders = new[]
                    {
                        new ShipmentLookupOrder { OrderId = "1", OrderItemIds = new IReadOnlyList<long>[] { new[] { 1L } } },
                    },
                },
                "tok"));
        Assert.Equal("50", ex.ErrorCode);
    }
}
