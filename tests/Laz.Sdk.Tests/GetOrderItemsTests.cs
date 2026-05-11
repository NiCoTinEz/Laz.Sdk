using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class GetOrderItemsTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    // Sample distilled from official /order/items/get response — covers mixed-form booleans
    // ("true", "True", "0", "1"), string-typed numerics, pick_up_store_info nested object,
    // and the open_hour string array.
    private const string SuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "pick_up_store_info":{
            "pick_up_store_address":"Ali Center, Shenzhen",
            "pick_up_store_name":"Alibaba",
            "pick_up_store_open_hour":["Sunday 9:00-18:00","Mon-Fri 8:00-20:00"],
            "pick_up_store_code":"d4b04804-9192-4a8c-8ed1-5ebcd7d3c067"
          },
          "tax_amount":"6.48",
          "sla_time_stamp":"2019-06-24T23:59:59+08:00",
          "show_giftwrapping_tag":"True",
          "voucher_seller":"0.00",
          "payment_time":"1697193374592",
          "voucher_code_seller":"X234",
          "voucher_code":"X3453",
          "package_id":"345",
          "buyer_id":"1001",
          "variation":"1",
          "is_cancel_pending":"true",
          "biz_group":"70100",
          "product_id":"12345",
          "voucher_code_platform":"Y123",
          "purchase_order_number":"345345",
          "sku":"BRSD#02",
          "gift_wrapping":"Happy birthday",
          "schedule_delivery_start_timeslot":"1719108000000",
          "order_type":"Normal",
          "invoice_number":"1342",
          "show_personalization_tag":"True",
          "can_escalate_pickup":"true",
          "cancel_trigger_time":"1754381842018",
          "cancel_return_initiator":"cancellation-customer",
          "shop_sku":"BE494HLAAUE3SGAMZ-39898",
          "is_reroute":"0",
          "stage_pay_status":"unpaid",
          "sku_id":"666",
          "tracking_code_pre":"23534",
          "order_item_id":"98108",
          "shop_id":"dawen dp",
          "order_flag":"GUATANTEE",
          "is_fbl":"0",
          "name":"Bean Rester Dooby Red",
          "delivery_option_sof":"0",
          "order_id":"31202",
          "fulfillment_sla":"NEXT_DAY",
          "need_cancel_confirm":"true",
          "status":"canceled",
          "product_main_image":"http://example/p.jpg",
          "voucher_platform":"0.00",
          "paid_price":"99.00",
          "product_detail_url":"http://example/detail.html",
          "warehouse_code":"WH-01",
          "promised_shipping_time":"2014-10-15 19:12:15 +0800",
          "shipping_type":"Dropshipping",
          "created_at":"2014-10-15 19:12:15 +0800",
          "supply_price":"99.0",
          "mp3_order":"True",
          "voucher_seller_lpi":"0.00",
          "shipping_fee_discount_platform":"0.00",
          "personalization":"Happy birthday",
          "wallet_credits":"0.00",
          "reverse_order_id":"704947701379284",
          "updated_at":"2014-10-15 19:12:15 +0800",
          "currency":"SGD",
          "shipping_provider_type":"standard",
          "voucher_platform_lpi":"0.00",
          "shipping_fee_original":"0.00",
          "schedule_delivery_end_timeslot":"1719140400000",
          "item_price":"99.00",
          "is_digital":"0",
          "shipping_service_cost":"0",
          "tracking_code":"456",
          "shipping_fee_discount_seller":"0.00",
          "shipping_amount":"0.00",
          "reason_detail":"reason detail",
          "return_status":"1",
          "semi_managed":"True",
          "shipment_provider":"LEL",
          "priority_fulfillment_tag":"Kirim secepat mungkin_null_null",
          "voucher_amount":"0.00",
          "supply_price_currency":"CNY",
          "digital_delivery_info":"delivery",
          "extra_attributes":null,
          "purchase_order_id":"3454",
          "reason":"reason"
        }
      ],
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task GetOrderItemsAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Orders.GetOrderItemsAsync(
            new GetOrderItemsRequest { OrderId = 31202 },
            "tok");

        var item = Assert.Single(response.Data!);
        Assert.Equal("98108",       item.OrderItemId);
        Assert.Equal("31202",       item.OrderId);
        Assert.Equal("BRSD#02",     item.Sku);
        Assert.Equal("99.00",       item.PaidPrice);
        Assert.Equal("SGD",         item.Currency);
        Assert.Equal("canceled",    item.Status);
        Assert.True(item.ShowGiftWrappingTag);   // "True"
        Assert.True(item.IsCancelPending);       // "true"
        Assert.True(item.Mp3Order);              // "True"
        Assert.False(item.IsDigital);            // "0"
        Assert.False(item.IsFbl);                // "0"
        Assert.False(item.IsReroute);            // "0"
        Assert.Equal(1, item.ReturnStatus);
        Assert.Equal(1697193374592L,  item.PaymentTime);
        Assert.Equal(1719108000000L,  item.ScheduleDeliveryStartTimeslot);
        Assert.Equal(1754381842018L,  item.CancelTriggerTime);

        Assert.NotNull(item.PickUpStoreInfo);
        Assert.Equal("Alibaba",            item.PickUpStoreInfo!.Name);
        Assert.Equal(2,                    item.PickUpStoreInfo.OpenHour!.Count);
    }

    [Fact]
    public async Task GetOrderItemsAsync_BuildsGetUrl_WithOrderId()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetOrderItemsAsync(new GetOrderItemsRequest { OrderId = 31202 }, "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/order/items/get?", url, StringComparison.Ordinal);
        Assert.Contains("order_id=31202", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetOrderItemsAsync_Throws_OnError()
    {
        const string ErrorBody = """{"code":"16","type":"ISV","message":"E016: Invalid Order ID","request_id":"r"}""";
        var client = NewClient(new TestHandler(ErrorBody));
        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Orders.GetOrderItemsAsync(new GetOrderItemsRequest { OrderId = 1 }, "tok"));
        Assert.Equal("16", ex.ErrorCode);
    }

    [Fact]
    public async Task GetOrderItemsAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Orders.GetOrderItemsAsync(new GetOrderItemsRequest { OrderId = 1 }, ""));
    }
}
