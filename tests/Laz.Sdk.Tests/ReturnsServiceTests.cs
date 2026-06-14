using Laz.Sdk.Models.Returns;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class ReturnsServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string DetailSuccessBody = """
    {
      "code":"0",
      "data":{
        "reverse_order_id":"12345",
        "order_id":"67890",
        "order_number":"ORD-001",
        "seller_id":"seller1",
        "buyer_id":"buyer1",
        "reverse_status":"pending",
        "reverse_type":"return",
        "request_reason":"wrong_item",
        "reason_detail":"Received wrong size",
        "created_at":"2026-01-15 10:00:00",
        "updated_at":"2026-01-15 12:00:00",
        "reverse_order_line_list":[
          {
            "reverse_order_line_id":"1001",
            "order_item_id":"2001",
            "sku":"SKU-001",
            "sku_name":"Product A",
            "shop_sku":"SHOP-A",
            "item_price":"50.00",
            "quantity":"1",
            "return_quantity":"1",
            "reverse_status":"pending",
            "reason":"wrong_item",
            "reason_detail":"Wrong size",
            "buyer_expected_action":"refund",
            "seller_action":"",
            "seller_action_detail":"",
            "return_shipment_provider":"DHL",
            "return_tracking_number":"TRACK123",
            "created_at":"2026-01-15 10:00:00",
            "updated_at":"2026-01-15 12:00:00"
          }
        ]
      },
      "request_id":"req-detail-1"
    }
    """;

    private const string HistorySuccessBody = """
    {
      "code":"0",
      "data":{
        "history_list":[
          {
            "reverse_order_line_id":"1001",
            "action":"created",
            "operator":"buyer1",
            "operator_type":"buyer",
            "remark":"Return requested",
            "created_at":"2026-01-15 10:00:00"
          },
          {
            "reverse_order_line_id":"1001",
            "action":"approved",
            "operator":"seller1",
            "operator_type":"seller",
            "remark":"Approved by seller",
            "created_at":"2026-01-15 12:00:00"
          }
        ],
        "page_size":"10",
        "page_number":"1",
        "total_count":"2"
      },
      "request_id":"req-hist-1"
    }
    """;

    private const string ReasonListSuccessBody = """
    {
      "code":"0",
      "data":[
        {"reason_id":"1","reason_name":"Wrong item sent"},
        {"reason_id":"2","reason_name":"Item damaged"},
        {"reason_id":"3","reason_name":"Item not as described"}
      ],
      "request_id":"req-reason-1"
    }
    """;

    private const string ReverseOrdersForSellerSuccessBody = """
    {
      "code":"0",
      "data":{
        "reverseOrders":[
          {
            "reverse_order_id":"12345",
            "order_id":"67890",
            "order_number":"ORD-001",
            "reverse_status":"pending",
            "reverse_type":"return",
            "request_reason":"wrong_item",
            "created_at":"2026-01-15 10:00:00",
            "updated_at":"2026-01-15 12:00:00",
            "reverse_order_line_list":[
              {
                "reverse_order_line_id":"1001",
                "order_item_id":"2001",
                "sku":"SKU-001",
                "sku_name":"Product A",
                "shop_sku":"SHOP-A",
                "reverse_status":"pending",
                "buyer_expected_action":"refund",
                "seller_action":""
              }
            ]
          }
        ],
        "total":"1",
        "totalPage":"1",
        "pageSize":"10",
        "currentPage":"1"
      },
      "request_id":"req-seller-1"
    }
    """;

    private const string CancelValidateSuccessBody = """
    {
      "code":"0",
      "data":{
        "can_cancel":true,
        "reason":""
      },
      "request_id":"req-val-1"
    }
    """;

    private const string CancelValidateCannotBody = """
    {
      "code":"0",
      "data":{
        "can_cancel":false,
        "reason":"Order is already shipped"
      },
      "request_id":"req-val-2"
    }
    """;

    private const string InitCancelSuccessBody = """
    {
      "code":"0",
      "data":{
        "success":true,
        "error_code":"",
        "error_msg":""
      },
      "request_id":"req-init-1"
    }
    """;

    private const string ReturnUpdateSuccessBody = """
    {
      "code":"0",
      "data":{
        "success":true,
        "error_code":"",
        "error_msg":""
      },
      "request_id":"req-upd-1"
    }
    """;

    // ──────────────────────────────────────────────
    // GetReverseOrderDetailAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetReverseOrderDetailAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(DetailSuccessBody);
        var client = NewClient(handler);

        var response = await client.Returns.GetReverseOrderDetailAsync(
            new GetReverseOrderDetailRequest { ReverseOrderId = 12345 },
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("req-detail-1", response.RequestId);
        Assert.NotNull(response.Data);
        Assert.Equal("12345", response.Data!.ReverseOrderId);
        Assert.Equal("67890", response.Data.OrderId);
        Assert.Equal("pending", response.Data.ReverseStatus);
        Assert.Equal("return", response.Data.ReverseType);
        Assert.NotNull(response.Data.ReverseOrderLineList);
        Assert.Single(response.Data.ReverseOrderLineList!);
        Assert.Equal("1001", response.Data.ReverseOrderLineList![0].ReverseOrderLineId);
        Assert.Equal("SKU-001", response.Data.ReverseOrderLineList[0].Sku);
    }

    [Fact]
    public async Task GetReverseOrderDetailAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(DetailSuccessBody);
        var client = NewClient(handler);

        await client.Returns.GetReverseOrderDetailAsync(
            new GetReverseOrderDetailRequest { ReverseOrderId = 12345 },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/order/reverse/return/detail/list?", url, StringComparison.Ordinal);
        Assert.Contains("reverse_order_id=12345", url, StringComparison.Ordinal);
        Assert.Contains("access_token=tok", url, StringComparison.Ordinal);
        Assert.Contains("sign=", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetReverseOrderDetailAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"32","type":"ISV","message":"Invalid reverse order id","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Returns.GetReverseOrderDetailAsync(
            new GetReverseOrderDetailRequest { ReverseOrderId = 99999 },
            "tok"));

        Assert.Equal("32", ex.ErrorCode);
        Assert.Contains("Invalid reverse order id", ex.ErrorMsg, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetReverseOrderDetailAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(DetailSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Returns.GetReverseOrderDetailAsync(
            new GetReverseOrderDetailRequest { ReverseOrderId = 1 },
            ""));
    }

    // ──────────────────────────────────────────────
    // GetReverseOrderHistoryListAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetReverseOrderHistoryListAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(HistorySuccessBody);
        var client = NewClient(handler);

        var response = await client.Returns.GetReverseOrderHistoryListAsync(
            new GetReverseOrderHistoryListRequest { ReverseOrderLineId = 1001 },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data!.HistoryList);
        Assert.Equal(2, response.Data.HistoryList!.Count);
        Assert.Equal("created", response.Data.HistoryList[0].Action);
        Assert.Equal("approved", response.Data.HistoryList[1].Action);
        Assert.Equal(10, response.Data.PageSize);
        Assert.Equal(1, response.Data.PageNumber);
        Assert.Equal(2, response.Data.TotalCount);
    }

    [Fact]
    public async Task GetReverseOrderHistoryListAsync_SendsOptionalPagingParams()
    {
        var handler = new TestHandler(HistorySuccessBody);
        var client = NewClient(handler);

        await client.Returns.GetReverseOrderHistoryListAsync(
            new GetReverseOrderHistoryListRequest
            {
                ReverseOrderLineId = 1001,
                PageSize = 20,
                PageNumber = 2,
            },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("reverse_order_line_id=1001", url, StringComparison.Ordinal);
        Assert.Contains("page_size=20", url, StringComparison.Ordinal);
        Assert.Contains("page_number=2", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetReverseOrderHistoryListAsync_OmitsPagingParams_WhenNull()
    {
        var handler = new TestHandler(HistorySuccessBody);
        var client = NewClient(handler);

        await client.Returns.GetReverseOrderHistoryListAsync(
            new GetReverseOrderHistoryListRequest { ReverseOrderLineId = 1001 },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("reverse_order_line_id=1001", url, StringComparison.Ordinal);
        Assert.DoesNotContain("page_size", url, StringComparison.Ordinal);
        Assert.DoesNotContain("page_number", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // GetReverseOrderReasonListAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetReverseOrderReasonListAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(ReasonListSuccessBody);
        var client = NewClient(handler);

        var response = await client.Returns.GetReverseOrderReasonListAsync(
            new GetReverseOrderReasonListRequest { ReverseOrderLineId = 1001 },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(3, response.Data!.Count);
        Assert.Equal("1", response.Data[0].ReasonId);
        Assert.Equal("Wrong item sent", response.Data[0].ReasonName);
        Assert.Equal("3", response.Data[2].ReasonId);
        Assert.Equal("Item not as described", response.Data[2].ReasonName);
    }

    [Fact]
    public async Task GetReverseOrderReasonListAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(ReasonListSuccessBody);
        var client = NewClient(handler);

        await client.Returns.GetReverseOrderReasonListAsync(
            new GetReverseOrderReasonListRequest { ReverseOrderLineId = 1001 },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/order/reverse/reason/list?", url, StringComparison.Ordinal);
        Assert.Contains("reverse_order_line_id=1001", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // GetReverseOrdersForSellerAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetReverseOrdersForSellerAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(ReverseOrdersForSellerSuccessBody);
        var client = NewClient(handler);

        var response = await client.Returns.GetReverseOrdersForSellerAsync(
            new GetReverseOrdersForSellerRequest(),
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data!.ReverseOrders);
        Assert.Single(response.Data.ReverseOrders!);
        Assert.Equal("12345", response.Data.ReverseOrders![0].ReverseOrderId);
        Assert.Equal("ORD-001", response.Data.ReverseOrders[0].OrderNumber);
        Assert.Equal(1, response.Data.Total);
        Assert.Equal(1, response.Data.CurrentPage);
    }

    [Fact]
    public async Task GetReverseOrdersForSellerAsync_SendsOptionalFilterParams()
    {
        var handler = new TestHandler(ReverseOrdersForSellerSuccessBody);
        var client = NewClient(handler);

        await client.Returns.GetReverseOrdersForSellerAsync(
            new GetReverseOrdersForSellerRequest
            {
                RequestTypeList = "return,refund",
                OfcStatusList = "pending",
                Offset = 0,
                PageSize = 20,
                CreatedAfter = 1700000000000,
                CreatedBefore = 1700100000000,
            },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("request_type_list=return%2Crefund", url, StringComparison.Ordinal);
        Assert.Contains("ofc_status_list=pending", url, StringComparison.Ordinal);
        Assert.Contains("offset=0", url, StringComparison.Ordinal);
        Assert.Contains("page_size=20", url, StringComparison.Ordinal);
        Assert.Contains("created_after=1700000000000", url, StringComparison.Ordinal);
        Assert.Contains("created_before=1700100000000", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // ReverseOrderCancelValidateAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task ReverseOrderCancelValidateAsync_ParsesCanCancelTrue()
    {
        var handler = new TestHandler(CancelValidateSuccessBody);
        var client = NewClient(handler);

        var response = await client.Returns.ReverseOrderCancelValidateAsync(
            new ReverseOrderCancelValidateRequest { OrderId = 67890 },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.CanCancel);
        Assert.Equal("", response.Data.Reason);
    }

    [Fact]
    public async Task ReverseOrderCancelValidateAsync_ParsesCanCancelFalse()
    {
        var handler = new TestHandler(CancelValidateCannotBody);
        var client = NewClient(handler);

        var response = await client.Returns.ReverseOrderCancelValidateAsync(
            new ReverseOrderCancelValidateRequest { OrderId = 67890 },
            "tok");

        Assert.False(response.Data!.CanCancel);
        Assert.Equal("Order is already shipped", response.Data.Reason);
    }

    [Fact]
    public async Task ReverseOrderCancelValidateAsync_SendsOptionalOrderItemId()
    {
        var handler = new TestHandler(CancelValidateSuccessBody);
        var client = NewClient(handler);

        await client.Returns.ReverseOrderCancelValidateAsync(
            new ReverseOrderCancelValidateRequest { OrderId = 67890, OrderItemId = 2001 },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("order_id=67890", url, StringComparison.Ordinal);
        Assert.Contains("order_item_id=2001", url, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // InitReverseOrderCancelAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task InitReverseOrderCancelAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(InitCancelSuccessBody);
        var client = NewClient(handler);

        var response = await client.Returns.InitReverseOrderCancelAsync(
            new InitReverseOrderCancelRequest { OrderId = 67890, Reason = "Buyer requested" },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    [Fact]
    public async Task InitReverseOrderCancelAsync_SendsPostWithFormData()
    {
        var handler = new TestHandler(InitCancelSuccessBody);
        var client = NewClient(handler);

        await client.Returns.InitReverseOrderCancelAsync(
            new InitReverseOrderCancelRequest { OrderId = 67890, Reason = "Buyer requested" },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("order_id=67890", body, StringComparison.Ordinal);
        Assert.Contains("reason=Buyer+requested", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task InitReverseOrderCancelAsync_SendsOptionalOrderItemId()
    {
        var handler = new TestHandler(InitCancelSuccessBody);
        var client = NewClient(handler);

        await client.Returns.InitReverseOrderCancelAsync(
            new InitReverseOrderCancelRequest { OrderId = 67890, Reason = "Buyer requested", OrderItemId = 2001 },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("order_item_id=2001", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task InitReverseOrderCancelAsync_RejectsEmptyReason()
    {
        var client = NewClient(new TestHandler(InitCancelSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Returns.InitReverseOrderCancelAsync(
            new InitReverseOrderCancelRequest { OrderId = 1, Reason = "" },
            "tok"));
    }

    // ──────────────────────────────────────────────
    // ReverseOrderReturnUpdateAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task ReverseOrderReturnUpdateAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(ReturnUpdateSuccessBody);
        var client = NewClient(handler);

        var response = await client.Returns.ReverseOrderReturnUpdateAsync(
            new ReverseOrderReturnUpdateRequest { ReverseOrderLineId = 1001, Action = "accept" },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    [Fact]
    public async Task ReverseOrderReturnUpdateAsync_SendsPostWithFormData()
    {
        var handler = new TestHandler(ReturnUpdateSuccessBody);
        var client = NewClient(handler);

        await client.Returns.ReverseOrderReturnUpdateAsync(
            new ReverseOrderReturnUpdateRequest { ReverseOrderLineId = 1001, Action = "accept" },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("reverse_order_line_id=1001", body, StringComparison.Ordinal);
        Assert.Contains("action=accept", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ReverseOrderReturnUpdateAsync_SendsOptionalReasonParams()
    {
        var handler = new TestHandler(ReturnUpdateSuccessBody);
        var client = NewClient(handler);

        await client.Returns.ReverseOrderReturnUpdateAsync(
            new ReverseOrderReturnUpdateRequest
            {
                ReverseOrderLineId = 1001,
                Action = "reject",
                Reason = "Item already returned",
                ReasonDetail = "Customer already received refund",
            },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("reason=Item+already+returned", body, StringComparison.Ordinal);
        Assert.Contains("reason_detail=Customer+already+received+refund", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ReverseOrderReturnUpdateAsync_RejectsEmptyAction()
    {
        var client = NewClient(new TestHandler(ReturnUpdateSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Returns.ReverseOrderReturnUpdateAsync(
            new ReverseOrderReturnUpdateRequest { ReverseOrderLineId = 1, Action = "" },
            "tok"));
    }

    [Fact]
    public async Task ReverseOrderReturnUpdateAsync_OmitsReasonParams_WhenNull()
    {
        var handler = new TestHandler(ReturnUpdateSuccessBody);
        var client = NewClient(handler);

        await client.Returns.ReverseOrderReturnUpdateAsync(
            new ReverseOrderReturnUpdateRequest { ReverseOrderLineId = 1001, Action = "accept" },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.DoesNotContain("reason=", body, StringComparison.Ordinal);
        Assert.DoesNotContain("reason_detail=", body, StringComparison.Ordinal);
    }

    // ──────────────────────────────────────────────
    // Regional gateway
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetReverseOrderDetailAsync_RegionalGateway_RespectsServerUrl()
    {
        var handler = new TestHandler(DetailSuccessBody);
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as", ServerUrl = UrlConstants.API_GATEWAY_URL_TH };
        var http = new HttpClient(handler);
        var client = new LazClient(http, opts);

        await client.Returns.GetReverseOrderDetailAsync(
            new GetReverseOrderDetailRequest { ReverseOrderId = 12345 },
            "tok");

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_TH + "/order/reverse/return/detail/list?",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }
}
