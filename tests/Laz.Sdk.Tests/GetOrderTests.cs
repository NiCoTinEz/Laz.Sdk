using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class GetOrderTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    // Sample distilled from official /order/get response. Note differences vs /orders/get:
    //   - gift_option wired as "0" (string number, not "false")
    //   - addressDistrict spelled correctly (not the addressDsitrict typo)
    private const string SuccessBody = """
    {
      "code":"0",
      "data":{
        "voucher":"0.00",
        "warehouse_code":"dropshipping",
        "order_number":"300034416",
        "created_at":"2014-10-15 18:36:05 +0800",
        "voucher_code":"3432",
        "gift_option":"0",
        "is_cancel_pending":"true",
        "shipping_fee_discount_platform":"0.00",
        "customer_last_name":"last_name",
        "updated_at":"2014-10-15 18:36:05 +0800",
        "promised_shipping_times":"2017-03-24 16:09:22",
        "price":"99.00",
        "national_registration_number":"1123",
        "shipping_fee_original":"0.00",
        "payment_method":"COD",
        "recipient_info":{"identify_no":"012345679","detail_address":"318 tanglin road","passport_no":"012345678"},
        "buyer_note":"red color",
        "customer_first_name":"First Name",
        "shipping_fee_discount_seller":"0.00",
        "shipping_fee":"0.00",
        "branch_number":"2222",
        "tax_code":"1234",
        "items_count":"1",
        "delivery_info":"1",
        "statuses":[],
        "address_billing":{
          "country":"Singapore","address1":"22 leonie hill road","city":"Singapore-Central",
          "phone2":"24***22","last_name":"Last Name","phone":"81***8","post_code":"239195",
          "addressDistrict":"D9","first_name":"First Name"
        },
        "extra_attributes":"{\"TaxInvoiceRequested\":\"true\"}",
        "order_id":"16090",
        "need_cancel_confirm":"true",
        "gift_message":"Gift",
        "remarks":"remarks",
        "address_shipping":{
          "country":"Singapore","address1":"318 tanglin road","city":"Singapore-Central",
          "phone":"94236248","post_code":"247979","first_name":"First Name","last_name":"Last Name"
        }
      },
      "request_id":"0ba2887315178178017221014"
    }
    """;

    [Fact]
    public async Task GetOrderAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Orders.GetOrderAsync(
            new GetOrderRequest { OrderId = 16090 },
            "tok");

        Assert.Equal("0", response.Code);
        var order = response.Data!;
        Assert.Equal("16090",       order.OrderId);
        Assert.Equal("300034416",   order.OrderNumber);
        Assert.Equal("99.00",       order.Price);
        Assert.False(order.GiftOption);       // "0" -> false (numeric-as-string boolean)
        Assert.True(order.IsCancelPending);   // "true" -> true
        Assert.Equal("D9", order.AddressBilling!.AddressDistrict);  // correctly-spelled key
        Assert.Equal("D9", order.AddressBilling.AddressDistrictFixed);
        Assert.Null(order.AddressBilling.AddressDistrictTypo);
        Assert.Equal("{\"TaxInvoiceRequested\":\"true\"}", order.ExtraAttributes);
    }

    [Fact]
    public async Task GetOrderAsync_BuildsGetUrl_WithOrderId()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Orders.GetOrderAsync(new GetOrderRequest { OrderId = 16090 }, "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/order/get?", url, StringComparison.Ordinal);
        Assert.Contains("order_id=16090",  url, StringComparison.Ordinal);
        Assert.Contains("access_token=tok", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetOrderAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Orders.GetOrderAsync(new GetOrderRequest { OrderId = 1 }, ""));
    }

    [Fact]
    public async Task GetOrderAsync_Throws_OnInvalidOrderId_Error()
    {
        const string ErrorBody = """{"code":"16","type":"ISV","message":"E016: \"abc\" Invalid Order ID","request_id":"r"}""";
        var client = NewClient(new TestHandler(ErrorBody));
        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Orders.GetOrderAsync(new GetOrderRequest { OrderId = 1 }, "tok"));
        Assert.Equal("16", ex.ErrorCode);
    }

    [Fact]
    public void LazAddress_AddressDistrict_PrefersFixed_OverTypo()
    {
        var both = new LazAddress { AddressDistrictTypo = "typo", AddressDistrictFixed = "fixed" };
        Assert.Equal("fixed", both.AddressDistrict);

        var typoOnly = new LazAddress { AddressDistrictTypo = "typo" };
        Assert.Equal("typo", typoOnly.AddressDistrict);

        var none = new LazAddress();
        Assert.Null(none.AddressDistrict);
    }
}
