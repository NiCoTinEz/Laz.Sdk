using Laz.Sdk.Models.Seller;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class SellerServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string GetSellerSuccessBody = """
    {
      "code":"0",
      "data":{
        "name_company":"Test Company Pte Ltd",
        "seller_id":"12345",
        "name":"Test Seller",
        "short_code":"TST",
        "logo_url":"https://example.com/logo.png",
        "email":"seller@example.com",
        "cb":"cb123",
        "location":"Singapore",
        "status":"active",
        "verified":true,
        "marketplaceEaseMode":false
      },
      "request_id":"req-seller-1"
    }
    """;

    private const string GetSellerMetricsSuccessBody = """
    {
      "code":"0",
      "data":{
        "main_category_name":"Fashion",
        "seller_id":"12345",
        "response_rate":"95.5",
        "response_time":"2.3",
        "ship_on_time":"98.1",
        "main_category_id":"1001",
        "positive_seller_rating":"92.0"
      },
      "request_id":"req-metrics-1"
    }
    """;

    private const string GetSellerPerformanceSuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "name":"Late shipment rate",
          "value":"2.5",
          "threshold":"5.0",
          "status":"pass",
          "metric":"percentage",
          "period":"last_30_days"
        },
        {
          "name":"Order cancellation rate",
          "value":"1.2",
          "threshold":"3.0",
          "status":"pass",
          "metric":"percentage",
          "period":"last_30_days"
        }
      ],
      "request_id":"req-perf-1"
    }
    """;

    private const string BatchQueryFollowStatusSuccessBody = """
    {
      "code":"0",
      "data":[
        {"buyer_id":"buyer1","follow":"1"},
        {"buyer_id":"buyer2","follow":"0"}
      ],
      "request_id":"req-follow-1"
    }
    """;

    private const string GetPickUpStoreListSuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "store_id":"store1",
          "store_name":"Pickup Point A",
          "address":"123 Main St",
          "phone":"+6512345678",
          "city":"Singapore",
          "district":"Central",
          "state":"Singapore",
          "country":"SG",
          "zip_code":"123456",
          "latitude":"1.3521",
          "longitude":"103.8198",
          "status":"active"
        }
      ],
      "request_id":"req-store-1"
    }
    """;

    private const string GetWarehouseBySellerIdSuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "warehouse_id":"wh1",
          "warehouse_name":"Main Warehouse",
          "warehouse_code":"MW-001",
          "address":"456 Industrial Ave",
          "status":"active",
          "type":"standard",
          "country":"SG"
        }
      ],
      "request_id":"req-wh-1"
    }
    """;

    private const string QueryWarehouseDetailSuccessBody = """
    {
      "code":"0",
      "data":{
        "warehouse_id":"wh1",
        "warehouse_name":"Main Warehouse",
        "warehouse_code":"MW-001",
        "address":"456 Industrial Ave",
        "city":"Singapore",
        "district":"East",
        "state":"Singapore",
        "country":"SG",
        "zip_code":"789012",
        "contact_person":"John Doe",
        "contact_phone":"+6598765432",
        "status":"active",
        "warehouse_type":"standard"
      },
      "request_id":"req-wh-detail-1"
    }
    """;

    private const string SellerPolicyFetchSuccessBody = """
    {
      "code":"0",
      "data":{
        "policy_id":"pol1",
        "policy_name":"Return Policy",
        "content":"Items can be returned within 30 days...",
        "version":"2.1",
        "effective_date":"2026-01-01"
      },
      "request_id":"req-policy-1"
    }
    """;

    private const string GetSellerRegisterInfoSuccessBody = """
    {
      "code":"0",
      "data":{
        "seller_id":"12345",
        "company_name":"Test Company Pte Ltd",
        "company_reg_no":"202500001A",
        "tax_id":"TAX12345",
        "address":"789 Business Rd",
        "phone":"+6512340000",
        "email":"info@testcompany.com",
        "country":"SG",
        "status":"registered"
      },
      "request_id":"req-reg-1"
    }
    """;

    private const string GetSubAddressSuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "address_id":"addr1",
          "address_name":"Office Address",
          "address_line":"10 Office Park",
          "city":"Singapore",
          "district":"Central",
          "state":"Singapore",
          "country":"SG",
          "zip_code":"123456",
          "phone":"+6512345678"
        }
      ],
      "request_id":"req-addr-1"
    }
    """;

    private const string PaymentBindingSuccessBody = """
    {
      "code":"0",
      "data":{
        "success":"true",
        "error_code":"",
        "error_msg":"",
        "binding_id":"bind123"
      },
      "request_id":"req-pay-1"
    }
    """;

    private const string SellerFieldVerifySuccessBody = """
    {
      "code":"0",
      "data":{
        "valid":"true",
        "error_code":"",
        "error_msg":""
      },
      "request_id":"req-verify-1"
    }
    """;

    private const string GetCountryInfoSuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "country_code":"SG",
          "country_name":"Singapore",
          "currency":"SGD",
          "timezone":"Asia/Singapore",
          "language":"en",
          "status":"active"
        },
        {
          "country_code":"MY",
          "country_name":"Malaysia",
          "currency":"MYR",
          "timezone":"Asia/Kuala_Lumpur",
          "language":"ms",
          "status":"active"
        }
      ],
      "request_id":"req-country-1"
    }
    """;

    // ──────────────────────────────────────────────
    // GetSellerAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetSellerAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetSellerSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.GetSellerAsync(accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("req-seller-1", response.RequestId);
        Assert.NotNull(response.Data);
        Assert.Equal("Test Company Pte Ltd", response.Data!.NameCompany);
        Assert.Equal("12345", response.Data.SellerId);
        Assert.Equal("Test Seller", response.Data.Name);
        Assert.Equal("TST", response.Data.ShortCode);
        Assert.Equal("seller@example.com", response.Data.Email);
        Assert.Equal("active", response.Data.Status);
        Assert.True(response.Data.Verified);
        Assert.False(response.Data.MarketplaceEaseMode);
    }

    [Fact]
    public async Task GetSellerAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(GetSellerSuccessBody);
        var client = NewClient(handler);

        await client.Seller.GetSellerAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/seller/get?", url, StringComparison.Ordinal);
        Assert.Contains("access_token=tok", url, StringComparison.Ordinal);
        Assert.Contains("sign=", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetSellerAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"32","type":"ISV","message":"Invalid seller","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Seller.GetSellerAsync("tok"));

        Assert.Equal("32", ex.ErrorCode);
        Assert.Contains("Invalid seller", ex.ErrorMsg, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetSellerAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(GetSellerSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetSellerAsync(""));
    }

    // ──────────────────────────────────────────────
    // GetSellerMetricsAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetSellerMetricsAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetSellerMetricsSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.GetSellerMetricsAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("Fashion", response.Data!.MainCategoryName);
        Assert.Equal("12345", response.Data.SellerId);
        Assert.Equal(95.5, response.Data.ResponseRate);
        Assert.Equal(2.3, response.Data.ResponseTime);
        Assert.Equal(98.1, response.Data.ShipOnTime);
        Assert.Equal(1001, response.Data.MainCategoryId);
        Assert.Equal(92.0, response.Data.PositiveSellerRating);
    }

    [Fact]
    public async Task GetSellerMetricsAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(GetSellerMetricsSuccessBody);
        var client = NewClient(handler);

        await client.Seller.GetSellerMetricsAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/seller/metrics/get?", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // GetSellerPerformanceAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetSellerPerformanceAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetSellerPerformanceSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.GetSellerPerformanceAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data!.Count);
        Assert.Equal("Late shipment rate", response.Data[0].Name);
        Assert.Equal("2.5", response.Data[0].Value);
        Assert.Equal("pass", response.Data[0].Status);
        Assert.Equal("Order cancellation rate", response.Data[1].Name);
    }

    [Fact]
    public async Task GetSellerPerformanceAsync_SendsOptionalLanguage()
    {
        var handler = new TestHandler(GetSellerPerformanceSuccessBody);
        var client = NewClient(handler);

        await client.Seller.GetSellerPerformanceAsync("tok", language: "en");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("language=en", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetSellerPerformanceAsync_OmitsLanguage_WhenNull()
    {
        var handler = new TestHandler(GetSellerPerformanceSuccessBody);
        var client = NewClient(handler);

        await client.Seller.GetSellerPerformanceAsync("tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.DoesNotContain("language", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // BatchQueryFollowStatusAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task BatchQueryFollowStatusAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(BatchQueryFollowStatusSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.BatchQueryFollowStatusAsync(
            new[] { "buyer1", "buyer2" }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data!.Count);
        Assert.Equal("buyer1", response.Data[0].BuyerId);
        Assert.Equal("1", response.Data[0].Follow);
        Assert.Equal("buyer2", response.Data[1].BuyerId);
        Assert.Equal("0", response.Data[1].Follow);
    }

    [Fact]
    public async Task BatchQueryFollowStatusAsync_SendsBuyerIds()
    {
        var handler = new TestHandler(BatchQueryFollowStatusSuccessBody);
        var client = NewClient(handler);

        await client.Seller.BatchQueryFollowStatusAsync(new[] { "buyer1", "buyer2" }, "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("buyer_ids=buyer1%2Cbuyer2", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task BatchQueryFollowStatusAsync_RejectsNullBuyerIds()
    {
        var client = NewClient(new TestHandler(BatchQueryFollowStatusSuccessBody));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.Seller.BatchQueryFollowStatusAsync(null!, "tok"));
    }

    // ──────────────────────────────────────────────
    // GetPickUpStoreListAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetPickUpStoreListAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetPickUpStoreListSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.GetPickUpStoreListAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data!);
        Assert.Equal("store1", response.Data![0].StoreId);
        Assert.Equal("Pickup Point A", response.Data[0].StoreName);
        Assert.Equal("active", response.Data[0].Status);
    }

    [Fact]
    public async Task GetPickUpStoreListAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(GetPickUpStoreListSuccessBody);
        var client = NewClient(handler);

        await client.Seller.GetPickUpStoreListAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/rc/store/list/get?", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // GetWarehouseBySellerIdAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetWarehouseBySellerIdAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetWarehouseBySellerIdSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.GetWarehouseBySellerIdAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data!);
        Assert.Equal("wh1", response.Data![0].WarehouseId);
        Assert.Equal("Main Warehouse", response.Data[0].WarehouseName);
        Assert.Equal("active", response.Data[0].Status);
    }

    // ──────────────────────────────────────────────
    // QueryWarehouseDetailAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task QueryWarehouseDetailAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(QueryWarehouseDetailSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.QueryWarehouseDetailAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("wh1", response.Data!.WarehouseId);
        Assert.Equal("Main Warehouse", response.Data.WarehouseName);
        Assert.Equal("Singapore", response.Data.City);
        Assert.Equal("John Doe", response.Data.ContactPerson);
    }

    // ──────────────────────────────────────────────
    // SellerPolicyFetchAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SellerPolicyFetchAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(SellerPolicyFetchSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.SellerPolicyFetchAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("pol1", response.Data!.PolicyId);
        Assert.Equal("Return Policy", response.Data.PolicyName);
        Assert.Equal("2.1", response.Data.Version);
    }

    // ──────────────────────────────────────────────
    // GetSellerRegisterInfoAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetSellerRegisterInfoAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetSellerRegisterInfoSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.GetSellerRegisterInfoAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("12345", response.Data!.SellerId);
        Assert.Equal("Test Company Pte Ltd", response.Data.CompanyName);
        Assert.Equal("202500001A", response.Data.CompanyRegNo);
        Assert.Equal("registered", response.Data.Status);
    }

    // ──────────────────────────────────────────────
    // GetSubAddressAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetSubAddressAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetSubAddressSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.GetSubAddressAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data!);
        Assert.Equal("addr1", response.Data![0].AddressId);
        Assert.Equal("Office Address", response.Data[0].AddressName);
        Assert.Equal("Singapore", response.Data[0].City);
    }

    // ──────────────────────────────────────────────
    // PaymentBindingAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task PaymentBindingAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(PaymentBindingSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.PaymentBindingAsync(
            new PaymentBindingRequest
            {
                PaymentMethod = "bank_transfer",
                AccountNo = "1234567890",
                AccountName = "Test Seller",
                BankCode = "BANK01",
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("bind123", response.Data!.BindingId);
    }

    [Fact]
    public async Task PaymentBindingAsync_SendsPostWithFormData()
    {
        var handler = new TestHandler(PaymentBindingSuccessBody);
        var client = NewClient(handler);

        await client.Seller.PaymentBindingAsync(
            new PaymentBindingRequest
            {
                PaymentMethod = "bank_transfer",
                AccountNo = "1234567890",
                AccountName = "Test Seller",
                BankCode = "BANK01",
            },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("payment_method=bank_transfer", body, StringComparison.Ordinal);
        Assert.Contains("account_no=1234567890", body, StringComparison.Ordinal);
        Assert.Contains("account_name=Test+Seller", body, StringComparison.Ordinal);
        Assert.Contains("bank_code=BANK01", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task PaymentBindingAsync_OmitsOptionalFields_WhenNull()
    {
        var handler = new TestHandler(PaymentBindingSuccessBody);
        var client = NewClient(handler);

        await client.Seller.PaymentBindingAsync(
            new PaymentBindingRequest
            {
                PaymentMethod = "bank_transfer",
                AccountNo = "1234567890",
            },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("payment_method=bank_transfer", body, StringComparison.Ordinal);
        Assert.Contains("account_no=1234567890", body, StringComparison.Ordinal);
        Assert.DoesNotContain("account_name", body, StringComparison.Ordinal);
        Assert.DoesNotContain("bank_code", body, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // SellerFieldVerifyAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SellerFieldVerifyAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(SellerFieldVerifySuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.SellerFieldVerifyAsync(
            new SellerFieldVerifyRequest
            {
                FieldName = "tax_id",
                FieldValue = "TAX12345",
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("true", response.Data!.Valid);
    }

    [Fact]
    public async Task SellerFieldVerifyAsync_SendsPostWithFormData()
    {
        var handler = new TestHandler(SellerFieldVerifySuccessBody);
        var client = NewClient(handler);

        await client.Seller.SellerFieldVerifyAsync(
            new SellerFieldVerifyRequest
            {
                FieldName = "tax_id",
                FieldValue = "TAX12345",
            },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("field_name=tax_id", body, StringComparison.Ordinal);
        Assert.Contains("field_value=TAX12345", body, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // GetCountryInfoAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetCountryInfoAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetCountryInfoSuccessBody);
        var client = NewClient(handler);

        var response = await client.Seller.GetCountryInfoAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data!.Count);
        Assert.Equal("SG", response.Data[0].CountryCode);
        Assert.Equal("Singapore", response.Data[0].CountryName);
        Assert.Equal("SGD", response.Data[0].Currency);
        Assert.Equal("MY", response.Data[1].CountryCode);
        Assert.Equal("Malaysia", response.Data[1].CountryName);
    }

    [Fact]
    public async Task GetCountryInfoAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(GetCountryInfoSuccessBody);
        var client = NewClient(handler);

        await client.Seller.GetCountryInfoAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/seller/country/info/get?", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // Common: Rejects empty access token
    // ──────────────────────────────────────────────

    [Fact]
    public async Task AllMethods_RejectEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(GetSellerSuccessBody));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetSellerAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetSellerMetricsAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetSellerPerformanceAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.BatchQueryFollowStatusAsync(new[] { "b1" }, ""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetPickUpStoreListAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetWarehouseBySellerIdAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.QueryWarehouseDetailAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.SellerPolicyFetchAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetSellerRegisterInfoAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetSubAddressAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.PaymentBindingAsync(new PaymentBindingRequest(), ""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.SellerFieldVerifyAsync(new SellerFieldVerifyRequest(), ""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Seller.GetCountryInfoAsync(""));
    }
}
