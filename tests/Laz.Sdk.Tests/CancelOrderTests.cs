using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class CancelOrderTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """{"code":"0","success":"true","request_id":"r"}""";

    [Fact]
    public async Task CancelAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Orders.CancelAsync(
            new CancelOrderRequest { OrderItemId = 140168, ReasonId = 15, ReasonDetail = "Out of stock" },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.True(response.Success);
        Assert.Equal("r", response.RequestId);
    }

    [Fact]
    public async Task CancelAsync_PostsAllParams_IncludingReasonDetail()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.CancelAsync(
            new CancelOrderRequest { OrderItemId = 140168, ReasonId = 15, ReasonDetail = "Out of stock" },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("order_item_id=140168",            body, StringComparison.Ordinal);
        Assert.Contains("reason_id=15",                    body, StringComparison.Ordinal);
        Assert.Contains("reason_detail=Out+of+stock",      body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task CancelAsync_OmitsReasonDetail_WhenNull()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.CancelAsync(
            new CancelOrderRequest { OrderItemId = 1, ReasonId = 2 },
            "tok");

        Assert.DoesNotContain("reason_detail=", handler.LastRequestBody!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task CancelAsync_Throws_OnError()
    {
        const string ErrorBody = """{"code":"22","type":"ISV","message":"E022: Invalid Reason","request_id":"r"}""";
        var client = NewClient(new TestHandler(ErrorBody));
        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Orders.CancelAsync(
                new CancelOrderRequest { OrderItemId = 1, ReasonId = 999 },
                "tok"));
        Assert.Equal("22", ex.ErrorCode);
    }
}
