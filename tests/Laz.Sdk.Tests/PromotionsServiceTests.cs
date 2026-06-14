using Laz.Sdk.Models.Promotions;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class PromotionsServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    // ──────────────────────────────────────────────
    // CreateVoucherAsync
    // ──────────────────────────────────────────────

    private const string CreateVoucherSuccessBody = """
    {
      "code":"0",
      "data":{"promotion_id":"12345"},
      "request_id":"req-vouch-create"
    }
    """;

    [Fact]
    public async Task CreateVoucherAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(CreateVoucherSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.CreateVoucherAsync(
            new CreateVoucherRequest
            {
                VoucherName = "Test Voucher",
                VoucherType = 1,
                VoucherDiscountType = 1,
                CollectStart = 1700000000000,
                PeriodStartTime = 1700000000000,
                PeriodEndTime  = 1700086400000,
                OfferingMoneyValueOff = 10.0m,
                CriteriaOverMoney = 100.0m,
                Apply = "all",
                Issued = 100,
                Limit = 1,
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("req-vouch-create", response.RequestId);
        Assert.NotNull(response.Data);
        Assert.Equal(12345, response.Data!.PromotionId);
    }

    [Fact]
    public async Task CreateVoucherAsync_SendsAllParams()
    {
        var handler = new TestHandler(CreateVoucherSuccessBody);
        var client = NewClient(handler);

        await client.Promotions.CreateVoucherAsync(
            new CreateVoucherRequest
            {
                VoucherName = "Test Voucher",
                VoucherType = 1,
                VoucherDiscountType = 1,
                CollectStart = 1700000000000,
                PeriodStartTime = 1700000000000,
                PeriodEndTime  = 1700086400000,
                OfferingMoneyValueOff = 10.0m,
                CriteriaOverMoney = 100.0m,
                Apply = "all",
                DisplayArea = "home",
                Issued = 100,
                Limit = 1,
                MaxDiscountOfferingMoneyValue = 50.0m,
                OfferingPercentageDiscountOff = 10.0m,
            },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("voucher_name=Test+Voucher", body);
        Assert.Contains("voucher_type=1", body);
        Assert.Contains("voucher_discount_type=1", body);
        Assert.Contains("collect_start=1700000000000", body);
        Assert.Contains("period_start_time=1700000000000", body);
        Assert.Contains("period_end_time=1700086400000", body);
        Assert.Contains("offering_money_value_off=10", body);
        Assert.Contains("criteria_over_money=100", body);
        Assert.Contains("apply=all", body);
        Assert.Contains("display_area=home", body);
        Assert.Contains("issued=100", body);
        Assert.Contains("limit=1", body);
        Assert.Contains("max_discount_offering_money_value=50", body);
        Assert.Contains("offering_percentage_discount_off=10", body);
    }

    [Fact]
    public async Task CreateVoucherAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"32","type":"ISV","message":"Invalid params","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Promotions.CreateVoucherAsync(new CreateVoucherRequest { VoucherName = "x", VoucherType = 1, VoucherDiscountType = 1, CollectStart = 1, PeriodStartTime = 1, PeriodEndTime = 2 }, "tok"));

        Assert.Equal("32", ex.ErrorCode);
    }

    // ──────────────────────────────────────────────
    // GetVoucherAsync
    // ──────────────────────────────────────────────

    private const string GetVoucherSuccessBody = """
    {
      "code":"0",
      "data":{
        "promotion_id":"12345",
        "voucher_name":"Test Voucher",
        "voucher_type":"1",
        "voucher_discount_type":"1",
        "criteria_over_money":"100.00",
        "offering_money_value_off":"10.00",
        "apply":"all",
        "collect_start":"1700000000000",
        "period_start_time":"1700000000000",
        "period_end_time":"1700086400000",
        "limit":"1",
        "issued":"100",
        "status":"active"
      },
      "request_id":"req-vouch-get"
    }
    """;

    [Fact]
    public async Task GetVoucherAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetVoucherSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.GetVoucherAsync(
            new GetVoucherRequest { VoucherType = 1, Id = 12345 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(12345, response.Data!.PromotionId);
        Assert.Equal("Test Voucher", response.Data.VoucherName);
        Assert.Equal("active", response.Data.Status);
    }

    [Fact]
    public async Task GetVoucherAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(GetVoucherSuccessBody);
        var client = NewClient(handler);

        await client.Promotions.GetVoucherAsync(
            new GetVoucherRequest { VoucherType = 1, Id = 12345 }, "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/promotion/voucher/get?", url, StringComparison.Ordinal);
        Assert.Contains("voucher_type=1", url, StringComparison.Ordinal);
        Assert.Contains("id=12345", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // GetVoucherListAsync
    // ──────────────────────────────────────────────

    private const string GetVoucherListSuccessBody = """
    {
      "code":"0",
      "data":{
        "voucher_list":[
          {"promotion_id":"1","voucher_name":"V1","voucher_type":"1","voucher_discount_type":"1","limit":"1","issued":"10","status":"active"},
          {"promotion_id":"2","voucher_name":"V2","voucher_type":"1","voucher_discount_type":"2","limit":"1","issued":"20","status":"active"}
        ],
        "total":"2"
      },
      "request_id":"req-vouch-list"
    }
    """;

    [Fact]
    public async Task GetVoucherListAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetVoucherListSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.GetVoucherListAsync(
            new GetVoucherListRequest { VoucherType = 1, Offset = 0, Limit = 10 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data!.Total);
        Assert.NotNull(response.Data.VoucherList);
        Assert.Equal(2, response.Data.VoucherList!.Count);
        Assert.Equal("V1", response.Data.VoucherList[0].VoucherName);
        Assert.Equal("V2", response.Data.VoucherList[1].VoucherName);
    }

    // ──────────────────────────────────────────────
    // ActivateVoucherAsync / DeactivateVoucherAsync
    // ──────────────────────────────────────────────

    private const string VoucherActionSuccessBody = """
    {
      "code":"0",
      "data":{"success":"true"},
      "request_id":"req-vouch-act"
    }
    """;

    [Fact]
    public async Task ActivateVoucherAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(VoucherActionSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.ActivateVoucherAsync(
            new VoucherActionRequest { VoucherType = 1, Id = 12345 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    [Fact]
    public async Task DeactivateVoucherAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(VoucherActionSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.DeactivateVoucherAsync(
            new VoucherActionRequest { VoucherType = 1, Id = 12345 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    // ──────────────────────────────────────────────
    // GetVoucherProductsAsync
    // ──────────────────────────────────────────────

    private const string GetVoucherProductsSuccessBody = """
    {
      "code":"0",
      "data":{
        "products":[
          {"item_id":"1001","sku_id":"2001","sku_name":"SKU A","item_name":"Item A"},
          {"item_id":"1002","sku_id":"2002","sku_name":"SKU B","item_name":"Item B"}
        ],
        "total":"2"
      },
      "request_id":"req-vouch-prod"
    }
    """;

    [Fact]
    public async Task GetVoucherProductsAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetVoucherProductsSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.GetVoucherProductsAsync(
            new GetVoucherProductsRequest { VoucherType = 1, Id = 12345 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data!.Total);
        Assert.NotNull(response.Data.Products);
        Assert.Equal(2, response.Data.Products!.Count);
        Assert.Equal(1001, response.Data.Products[0].ItemId);
        Assert.Equal(2001, response.Data.Products[0].SkuId);
    }

    // ──────────────────────────────────────────────
    // AddVoucherSkuAsync / RemoveVoucherSkuAsync
    // ──────────────────────────────────────────────

    private const string AddVoucherSkuSuccessBody = """
    {
      "code":"0",
      "data":{"failed_sku_ids":[]},
      "request_id":"req-vouch-sku-add"
    }
    """;

    [Fact]
    public async Task AddVoucherSkuAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(AddVoucherSkuSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.AddVoucherSkuAsync(
            new VoucherSkuRequest { VoucherType = 1, Id = 12345, SkuIds = new[] { 2001L, 2002L } }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data!.FailedSkuIds);
        Assert.Empty(response.Data.FailedSkuIds!);
    }

    [Fact]
    public async Task AddVoucherSkuAsync_SendsSkuIdsAsJsonArray()
    {
        var handler = new TestHandler(AddVoucherSkuSuccessBody);
        var client = NewClient(handler);

        await client.Promotions.AddVoucherSkuAsync(
            new VoucherSkuRequest { VoucherType = 1, Id = 12345, SkuIds = new[] { 2001L, 2002L } }, "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("sku_ids=%5B2001%2C2002%5D", body); // URL-encoded [2001,2002]
    }

    private const string RemoveVoucherSkuSuccessBody = """
    {
      "code":"0",
      "data":{"success":"true"},
      "request_id":"req-vouch-sku-rm"
    }
    """;

    [Fact]
    public async Task RemoveVoucherSkuAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(RemoveVoucherSkuSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.RemoveVoucherSkuAsync(
            new VoucherSkuRequest { VoucherType = 1, Id = 12345, SkuIds = new[] { 2001L } }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    // ──────────────────────────────────────────────
    // CreateFreeShippingAsync
    // ──────────────────────────────────────────────

    private const string CreateFreeShippingSuccessBody = """
    {
      "code":"0",
      "data":{"promotion_id":"54321"},
      "request_id":"req-fs-create"
    }
    """;

    [Fact]
    public async Task CreateFreeShippingAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(CreateFreeShippingSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.CreateFreeShippingAsync(
            new CreateFreeShippingRequest
            {
                FreeShippingName = "Free Ship Promo",
                FreeShippingType = 1,
                StartTime = 1700000000000,
                EndTime = 1700086400000,
                Apply = "all",
                CriteriaOverMoney = 50.0m,
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(54321, response.Data!.PromotionId);
    }

    // ──────────────────────────────────────────────
    // GetFreeShippingAsync
    // ──────────────────────────────────────────────

    private const string GetFreeShippingSuccessBody = """
    {
      "code":"0",
      "data":{
        "promotion_id":"54321",
        "free_shipping_name":"Free Ship Promo",
        "free_shipping_type":"1",
        "start_time":"1700000000000",
        "end_time":"1700086400000",
        "apply":"all",
        "criteria_over_money":"50.00",
        "status":"active"
      },
      "request_id":"req-fs-get"
    }
    """;

    [Fact]
    public async Task GetFreeShippingAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetFreeShippingSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.GetFreeShippingAsync(
            new GetFreeShippingRequest { Id = 54321 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(54321, response.Data!.PromotionId);
        Assert.Equal("Free Ship Promo", response.Data.FreeShippingName);
        Assert.Equal("active", response.Data.Status);
    }

    [Fact]
    public async Task GetFreeShippingAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(GetFreeShippingSuccessBody);
        var client = NewClient(handler);

        await client.Promotions.GetFreeShippingAsync(
            new GetFreeShippingRequest { Id = 54321 }, "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/promotion/freeshipping/get?", url, StringComparison.Ordinal);
        Assert.Contains("id=54321", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // GetFreeShippingListAsync
    // ──────────────────────────────────────────────

    private const string GetFreeShippingListSuccessBody = """
    {
      "code":"0",
      "data":{
        "free_shipping_list":[
          {"promotion_id":"1","free_shipping_name":"FS1","free_shipping_type":"1","status":"active"},
          {"promotion_id":"2","free_shipping_name":"FS2","free_shipping_type":"1","status":"active"}
        ],
        "total":"2"
      },
      "request_id":"req-fs-list"
    }
    """;

    [Fact]
    public async Task GetFreeShippingListAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetFreeShippingListSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.GetFreeShippingListAsync(
            new GetFreeShippingListRequest { Offset = 0, Limit = 10 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data!.Total);
        Assert.NotNull(response.Data.FreeShippingList);
        Assert.Equal(2, response.Data.FreeShippingList!.Count);
        Assert.Equal("FS1", response.Data.FreeShippingList[0].FreeShippingName);
    }

    // ──────────────────────────────────────────────
    // ActivateFreeShippingAsync / DeactivateFreeShippingAsync
    // ──────────────────────────────────────────────

    private const string FreeShippingActionSuccessBody = """
    {
      "code":"0",
      "data":{"success":"true"},
      "request_id":"req-fs-act"
    }
    """;

    [Fact]
    public async Task ActivateFreeShippingAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(FreeShippingActionSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.ActivateFreeShippingAsync(
            new FreeShippingActionRequest { Id = 54321 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    [Fact]
    public async Task DeactivateFreeShippingAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(FreeShippingActionSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.DeactivateFreeShippingAsync(
            new FreeShippingActionRequest { Id = 54321 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    // ──────────────────────────────────────────────
    // GetFreeShippingProductsAsync
    // ──────────────────────────────────────────────

    private const string GetFreeShippingProductsSuccessBody = """
    {
      "code":"0",
      "data":{
        "products":[
          {"item_id":"1001","sku_id":"2001","sku_name":"SKU A","item_name":"Item A"}
        ],
        "total":"1"
      },
      "request_id":"req-fs-prod"
    }
    """;

    [Fact]
    public async Task GetFreeShippingProductsAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetFreeShippingProductsSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.GetFreeShippingProductsAsync(
            new GetFreeShippingProductsRequest { Id = 54321 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(1, response.Data!.Total);
        Assert.NotNull(response.Data.Products);
        Assert.Single(response.Data.Products!);
        Assert.Equal(1001, response.Data.Products![0].ItemId);
    }

    // ──────────────────────────────────────────────
    // AddFreeShippingSkuAsync / RemoveFreeShippingSkuAsync
    // ──────────────────────────────────────────────

    private const string AddFreeShippingSkuSuccessBody = """
    {
      "code":"0",
      "data":{"failed_sku_ids":[]},
      "request_id":"req-fs-sku-add"
    }
    """;

    [Fact]
    public async Task AddFreeShippingSkuAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(AddFreeShippingSkuSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.AddFreeShippingSkuAsync(
            new FreeShippingSkuRequest { Id = 54321, SkuIds = new[] { 2001L } }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data!.FailedSkuIds!);
    }

    private const string RemoveFreeShippingSkuSuccessBody = """
    {
      "code":"0",
      "data":{"success":"true"},
      "request_id":"req-fs-sku-rm"
    }
    """;

    [Fact]
    public async Task RemoveFreeShippingSkuAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(RemoveFreeShippingSkuSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.RemoveFreeShippingSkuAsync(
            new FreeShippingSkuRequest { Id = 54321, SkuIds = new[] { 2001L } }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    // ──────────────────────────────────────────────
    // CreateFlexiComboAsync
    // ──────────────────────────────────────────────

    private const string CreateFlexiComboSuccessBody = """
    {
      "code":"0",
      "data":{"promotion_id":"99999"},
      "request_id":"req-fc-create"
    }
    """;

    [Fact]
    public async Task CreateFlexiComboAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(CreateFlexiComboSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.CreateFlexiComboAsync(
            new CreateFlexiComboRequest
            {
                FlexiComboName = "Bundle Deal",
                FlexiComboType = 1,
                StartTime = 1700000000000,
                EndTime = 1700086400000,
                DiscountType = 1,
                DiscountValue = 20.0m,
                MinSpend = 100.0m,
                MaxDiscount = 50.0m,
                Apply = "all",
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(99999, response.Data!.PromotionId);
    }

    // ──────────────────────────────────────────────
    // GetFlexiComboDetailsAsync
    // ──────────────────────────────────────────────

    private const string GetFlexiComboDetailsSuccessBody = """
    {
      "code":"0",
      "data":{
        "promotion_id":"99999",
        "flexi_combo_name":"Bundle Deal",
        "flexi_combo_type":"1",
        "start_time":"1700000000000",
        "end_time":"1700086400000",
        "discount_type":"1",
        "discount_value":"20.00",
        "min_spend":"100.00",
        "max_discount":"50.00",
        "apply":"all",
        "status":"active"
      },
      "request_id":"req-fc-detail"
    }
    """;

    [Fact]
    public async Task GetFlexiComboDetailsAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetFlexiComboDetailsSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.GetFlexiComboDetailsAsync(
            new GetFlexiComboDetailsRequest { Id = 99999 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(99999, response.Data!.PromotionId);
        Assert.Equal("Bundle Deal", response.Data.FlexiComboName);
        Assert.Equal("active", response.Data.Status);
    }

    [Fact]
    public async Task GetFlexiComboDetailsAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(GetFlexiComboDetailsSuccessBody);
        var client = NewClient(handler);

        await client.Promotions.GetFlexiComboDetailsAsync(
            new GetFlexiComboDetailsRequest { Id = 99999 }, "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/promotion/flexicombo/details?", url, StringComparison.Ordinal);
        Assert.Contains("id=99999", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // GetFlexiComboListAsync
    // ──────────────────────────────────────────────

    private const string GetFlexiComboListSuccessBody = """
    {
      "code":"0",
      "data":{
        "flexi_combo_list":[
          {"promotion_id":"1","flexi_combo_name":"FC1","flexi_combo_type":"1","discount_type":"1","status":"active"},
          {"promotion_id":"2","flexi_combo_name":"FC2","flexi_combo_type":"2","discount_type":"1","status":"active"}
        ],
        "total":"2"
      },
      "request_id":"req-fc-list"
    }
    """;

    [Fact]
    public async Task GetFlexiComboListAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetFlexiComboListSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.GetFlexiComboListAsync(
            new GetFlexiComboListRequest { Offset = 0, Limit = 10 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data!.Total);
        Assert.NotNull(response.Data.FlexiComboList);
        Assert.Equal(2, response.Data.FlexiComboList!.Count);
        Assert.Equal("FC1", response.Data.FlexiComboList[0].FlexiComboName);
    }

    // ──────────────────────────────────────────────
    // ActivateFlexiComboAsync / DeactivateFlexiComboAsync
    // ──────────────────────────────────────────────

    private const string FlexiComboActionSuccessBody = """
    {
      "code":"0",
      "data":{"success":"true"},
      "request_id":"req-fc-act"
    }
    """;

    [Fact]
    public async Task ActivateFlexiComboAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(FlexiComboActionSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.ActivateFlexiComboAsync(
            new FlexiComboActionRequest { Id = 99999 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    [Fact]
    public async Task DeactivateFlexiComboAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(FlexiComboActionSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.DeactivateFlexiComboAsync(
            new FlexiComboActionRequest { Id = 99999 }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    // ──────────────────────────────────────────────
    // AddFlexiComboProductsAsync / RemoveFlexiComboProductsAsync
    // ──────────────────────────────────────────────

    private const string AddFlexiComboProductsSuccessBody = """
    {
      "code":"0",
      "data":{"failed_product_ids":[]},
      "request_id":"req-fc-prod-add"
    }
    """;

    [Fact]
    public async Task AddFlexiComboProductsAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(AddFlexiComboProductsSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.AddFlexiComboProductsAsync(
            new FlexiComboProductsRequest { Id = 99999, ProductIds = new[] { 1001L, 1002L } }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data!.FailedProductIds!);
    }

    private const string RemoveFlexiComboProductsSuccessBody = """
    {
      "code":"0",
      "data":{"success":"true"},
      "request_id":"req-fc-prod-rm"
    }
    """;

    [Fact]
    public async Task RemoveFlexiComboProductsAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(RemoveFlexiComboProductsSuccessBody);
        var client = NewClient(handler);

        var response = await client.Promotions.RemoveFlexiComboProductsAsync(
            new FlexiComboProductsRequest { Id = 99999, ProductIds = new[] { 1001L } }, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    // ──────────────────────────────────────────────
    // Common error handling
    // ──────────────────────────────────────────────

    [Fact]
    public async Task AllMethods_RejectEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(CreateVoucherSuccessBody));

        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Promotions.CreateVoucherAsync(new CreateVoucherRequest { VoucherName = "x", VoucherType = 1, VoucherDiscountType = 1, CollectStart = 1, PeriodStartTime = 1, PeriodEndTime = 2 }, ""));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Promotions.GetVoucherAsync(new GetVoucherRequest { VoucherType = 1, Id = 1 }, ""));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Promotions.GetVoucherListAsync(new GetVoucherListRequest { VoucherType = 1 }, ""));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Promotions.ActivateVoucherAsync(new VoucherActionRequest { VoucherType = 1, Id = 1 }, ""));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Promotions.CreateFreeShippingAsync(new CreateFreeShippingRequest { FreeShippingName = "x", FreeShippingType = 1, StartTime = 1, EndTime = 2 }, ""));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Promotions.GetFreeShippingAsync(new GetFreeShippingRequest { Id = 1 }, ""));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Promotions.CreateFlexiComboAsync(new CreateFlexiComboRequest { FlexiComboName = "x", FlexiComboType = 1, StartTime = 1, EndTime = 2 }, ""));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Promotions.GetFlexiComboDetailsAsync(new GetFlexiComboDetailsRequest { Id = 1 }, ""));
    }
}
