using System.Globalization;
using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class GetOrdersTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        return new LazClient(new HttpClient(handler), opts);
    }

    // Sample distilled from official /orders/get response.
    private const string SuccessBody = """
    {
      "code":"0",
      "data":{
        "count":"10",
        "countTotal":"500",
        "orders":[
          {
            "voucher_platform":"0.00",
            "voucher":"0.00",
            "warehouse_code":"dropshipping",
            "order_number":"491253082180001",
            "voucher_seller":"0.00",
            "created_at":"2018-02-09T22:44:30+08:00",
            "voucher_code":"1234",
            "gift_option":"false",
            "is_cancel_pending":"true",
            "shipping_fee_discount_platform":"0.00",
            "customer_last_name":"last_name",
            "promised_shipping_times":"shipping_time",
            "updated_at":"2018-02-09T22:44:30+08:00",
            "price":"106.00",
            "national_registration_number":"1",
            "shipping_fee_original":"0.00",
            "payment_method":"COD",
            "address_updated_at":null,
            "recipient_info":{"identify_no":"012345679","detail_address":"318 tanglin road","passport_no":"012345678"},
            "buyer_note":"red color",
            "customer_first_name":"Ha Hung",
            "shipping_fee_discount_seller":"0.00",
            "shipping_fee":"0.54",
            "branch_number":"2222",
            "tax_code":"562562",
            "items_count":"2",
            "delivery_info":"delivery",
            "statuses":["shipped"],
            "address_billing":{"country":"Singapore","address1":"1 CHANGI VILLAGE ROAD, 11","city":"Singapore-Singapore-500001","post_code":"500001","first_name":"Ha Hung","last_name":"last_name","phone":"61****7"},
            "extra_attributes":null,
            "order_id":"491253082180001",
            "need_cancel_confirm":"true",
            "remarks":"remarks",
            "gift_message":"1",
            "address_shipping":{"country":"Singapore","address1":"1 CHANGI VILLAGE ROAD, 11","city":"Singapore-Singapore-500001","post_code":"500001","first_name":"Ha Hung","last_name":"last_name","phone":"6****67"}
          }
        ]
      },
      "request_id":"0ba2887315178178017221014"
    }
    """;

    [Fact]
    public async Task GetOrdersAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Orders.GetOrdersAsync(
            new GetOrdersRequest { CreatedAfter = new DateTimeOffset(2017, 2, 10, 9, 0, 0, TimeSpan.FromHours(8)) },
            accessToken: "tok");

        Assert.Equal(10,  response.Data!.Count);
        Assert.Equal(500, response.Data.CountTotal);
        var order = Assert.Single(response.Data.Orders!);
        Assert.Equal("491253082180001", order.OrderId);
        Assert.Equal("COD",             order.PaymentMethod);
        Assert.Equal("106.00",          order.Price);
        Assert.False(order.GiftOption);          // wired as "false"
        Assert.True(order.IsCancelPending);      // wired as "true"
        Assert.True(order.NeedCancelConfirm);
        Assert.Equal(2, order.ItemsCount);
        Assert.Single(order.Statuses!);
        Assert.Equal("shipped", order.Statuses![0]);
        Assert.Equal("Singapore", order.AddressBilling!.Country);
        Assert.Equal("6****67",   order.AddressShipping!.Phone);
        Assert.Equal("012345678", order.RecipientInfo!.PassportNo);
    }

    [Fact]
    public async Task GetOrdersAsync_BuildsGetUrl_WithAllFilters()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetOrdersAsync(
            new GetOrdersRequest
            {
                CreatedAfter  = new DateTimeOffset(2017, 2, 10,  9, 0, 0, TimeSpan.FromHours(8)),
                CreatedBefore = new DateTimeOffset(2018, 2, 10, 16, 0, 0, TimeSpan.FromHours(8)),
                UpdateAfter   = new DateTimeOffset(2017, 2, 10,  9, 0, 0, TimeSpan.FromHours(8)),
                UpdateBefore  = new DateTimeOffset(2018, 2, 10, 16, 0, 0, TimeSpan.FromHours(8)),
                Status        = OrderStatuses.Shipped,
                Limit         = 10,
                Offset        = 0,
                SortBy        = OrderSortBy.UpdatedAt,
                SortDirection = OrderSortDirection.Desc,
            },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/orders/get?", url, StringComparison.Ordinal);
        Assert.Contains("status=shipped",                                              url, StringComparison.Ordinal);
        Assert.Contains("limit=10",                                                    url, StringComparison.Ordinal);
        Assert.Contains("offset=0",                                                    url, StringComparison.Ordinal);
        Assert.Contains("sort_by=updated_at",                                          url, StringComparison.Ordinal);
        Assert.Contains("sort_direction=DESC",                                         url, StringComparison.Ordinal);
        Assert.Contains("created_after=" + Uri.EscapeDataString("2017-02-10T09:00:00+08:00"),  url, StringComparison.Ordinal);
        Assert.Contains("created_before=" + Uri.EscapeDataString("2018-02-10T16:00:00+08:00"), url, StringComparison.Ordinal);
        Assert.Contains("update_after=" + Uri.EscapeDataString("2017-02-10T09:00:00+08:00"),   url, StringComparison.Ordinal);
        Assert.Contains("update_before=" + Uri.EscapeDataString("2018-02-10T16:00:00+08:00"),  url, StringComparison.Ordinal);
        Assert.Contains("access_token=tok",                                            url, StringComparison.Ordinal);
        Assert.Contains("sign=",                                                       url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetOrdersAsync_OnlyUpdateAfter_Satisfies_Required_FilterRule()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await client.Orders.GetOrdersAsync(
            new GetOrdersRequest { UpdateAfter = DateTimeOffset.UtcNow.AddDays(-1) },
            "tok"); // no throw
    }

    [Fact]
    public async Task GetOrdersAsync_RejectsRequest_MissingBothAfterFilters()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Orders.GetOrdersAsync(
            new GetOrdersRequest { Status = OrderStatuses.Shipped },
            "tok"));
    }

    [Fact]
    public async Task GetOrdersAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Orders.GetOrdersAsync(
            new GetOrdersRequest { UpdateAfter = DateTimeOffset.UtcNow.AddDays(-1) },
            ""));
    }

    [Fact]
    public async Task GetOrdersAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"36","type":"ISV","message":"E036: Invalid status filter","request_id":"r"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Orders.GetOrdersAsync(
            new GetOrdersRequest { UpdateAfter = DateTimeOffset.UtcNow.AddDays(-1) },
            "tok"));
        Assert.Equal("36", ex.ErrorCode);
    }

    [Theory]
    [InlineData(OrderSortBy.CreatedAt, "sort_by=created_at")]
    [InlineData(OrderSortBy.UpdatedAt, "sort_by=updated_at")]
    public async Task GetOrdersAsync_MapsSortBy(OrderSortBy by, string expectedFragment)
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetOrdersAsync(
            new GetOrdersRequest { UpdateAfter = DateTimeOffset.UtcNow.AddDays(-1), SortBy = by },
            "tok");

        Assert.Contains(expectedFragment, handler.LastRequest!.RequestUri!.ToString(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(OrderSortDirection.Asc,  "sort_direction=ASC")]
    [InlineData(OrderSortDirection.Desc, "sort_direction=DESC")]
    public async Task GetOrdersAsync_MapsSortDirection(OrderSortDirection dir, string expectedFragment)
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetOrdersAsync(
            new GetOrdersRequest { UpdateAfter = DateTimeOffset.UtcNow.AddDays(-1), SortDirection = dir },
            "tok");

        Assert.Contains(expectedFragment, handler.LastRequest!.RequestUri!.ToString(), StringComparison.Ordinal);
    }
}
