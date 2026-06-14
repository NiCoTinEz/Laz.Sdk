using Laz.Sdk.Models.Finance;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class FinanceServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string PayoutSuccessBody = """
    {
      "code":"0",
      "data":[
        {
          "closing_balance":"100.00",
          "payout":"50.00",
          "paid":"50.00",
          "statement_number":"STMT-001",
          "created_at":"2018-01-15 10:00:00",
          "updated_at":"2018-01-15 12:00:00",
          "opening_balance":"50.00",
          "item_revenue":"200.00",
          "shipment_fee":"-10.00",
          "refunds":"-20.00",
          "commission_fee":"-15.00",
          "transaction_fee":"-5.00",
          "other_fee":"0.00",
          "adjustment":"0.00",
          "payment_voucher_number":"PV-001",
          "payment_voucher_date":"2018-01-20",
          "payment_received":"50.00",
          "paid_status":"paid",
          "currency":"SGD"
        }
      ],
      "request_id":"req-payout-001"
    }
    """;

    private const string TransactionsSuccessBody = """
    {
      "code":"0",
      "data":{
        "total":"100",
        "currentPage":"1",
        "pageSize":"10",
        "totalPage":"10",
        "list":[
          {
            "transaction_number":"TXN-001",
            "transaction_type":"Order",
            "sub_transaction_type":"Sale",
            "transaction_time":"2018-01-15 10:00:00",
            "order_number":"ORD-001",
            "debit_amount":"0.00",
            "credit_amount":"100.00",
            "balance":"200.00",
            "remark":"Order payment",
            "currency":"SGD"
          }
        ]
      },
      "request_id":"req-txn-001"
    }
    """;

    private const string LogisticsFeeSuccessBody = """
    {
      "code":"0",
      "data":{
        "total":"50",
        "currentPage":"1",
        "pageSize":"10",
        "list":[
          {
            "order_number":"ORD-001",
            "order_item_id":"12345",
            "shipment_provider":"DHL",
            "tracking_number":"TRACK-001",
            "fee_type":"Shipping",
            "fee_amount":"5.00",
            "currency":"SGD",
            "charge_time":"2018-01-15 10:00:00",
            "remark":"Standard shipping"
          }
        ]
      },
      "request_id":"req-log-001"
    }
    """;

    private const string TransactionDetailSuccessBody = """
    {
      "code":"0",
      "data":{
        "total":"200",
        "currentPage":"1",
        "pageSize":"20",
        "list":[
          {
            "transaction_number":"TXN-D-001",
            "transaction_type":"Payout",
            "transaction_time":"2018-01-15 10:00:00",
            "order_number":"ORD-002",
            "debit_amount":"0.00",
            "credit_amount":"500.00",
            "balance":"1000.00",
            "remark":"Payout received",
            "currency":"SGD"
          }
        ]
      },
      "request_id":"req-td-001"
    }
    """;

    // ─── GetPayoutStatusAsync ───────────────────────────────────────────

    [Fact]
    public async Task GetPayoutStatusAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(PayoutSuccessBody);
        var client = NewClient(handler);

        var response = await client.Finance.GetPayoutStatusAsync(
            new GetPayoutStatusRequest { CreatedAfter = "2018-01-01" },
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.Equal("req-payout-001", response.RequestId);
        Assert.NotNull(response.Data);
        var stmt = Assert.Single(response.Data!);
        Assert.Equal("STMT-001", stmt.StatementNumber);
        Assert.Equal("100.00", stmt.ClosingBalance);
        Assert.Equal("50.00", stmt.Payout);
        Assert.Equal("SGD", stmt.Currency);
    }

    [Fact]
    public async Task GetPayoutStatusAsync_BuildsGetUrl_WithExpectedQuery()
    {
        var handler = new TestHandler(PayoutSuccessBody);
        var client = NewClient(handler);

        await client.Finance.GetPayoutStatusAsync(
            new GetPayoutStatusRequest { CreatedAfter = "2018-01-01" },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/finance/payout/status/get?", url, StringComparison.Ordinal);
        Assert.Contains("created_after=2018-01-01", url, StringComparison.Ordinal);
        Assert.Contains("access_token=tok", url, StringComparison.Ordinal);
        Assert.Contains("sign=", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetPayoutStatusAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"400","type":"ISV","message":"Invalid date","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Finance.GetPayoutStatusAsync(
            new GetPayoutStatusRequest { CreatedAfter = "bad-date" },
            "tok"));

        Assert.Equal("400", ex.ErrorCode);
        Assert.Contains("Invalid date", ex.ErrorMsg, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetPayoutStatusAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(PayoutSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Finance.GetPayoutStatusAsync(
            new GetPayoutStatusRequest { CreatedAfter = "2018-01-01" },
            ""));
    }

    [Fact]
    public async Task GetPayoutStatusAsync_RejectsEmptyCreatedAfter()
    {
        var client = NewClient(new TestHandler(PayoutSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Finance.GetPayoutStatusAsync(
            new GetPayoutStatusRequest { CreatedAfter = "" },
            "tok"));
    }

    // ─── QueryAccountTransactionsAsync ──────────────────────────────────

    [Fact]
    public async Task QueryAccountTransactionsAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(TransactionsSuccessBody);
        var client = NewClient(handler);

        var response = await client.Finance.QueryAccountTransactionsAsync(
            new QueryAccountTransactionsRequest
            {
                PageSize = 10,
                StartTime = "20180101",
                EndTime = "20180131",
                PageNum = 1,
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(100, response.Data.Total);
        Assert.Equal(1, response.Data.CurrentPage);
        Assert.Equal(10, response.Data.PageSize);
        Assert.Equal(10, response.Data.TotalPage);
        Assert.NotNull(response.Data.List);
        var txn = Assert.Single(response.Data.List);
        Assert.Equal("TXN-001", txn.TransactionNumber);
        Assert.Equal("Order", txn.TransactionType);
    }

    [Fact]
    public async Task QueryAccountTransactionsAsync_PostsRequiredParams()
    {
        var handler = new TestHandler(TransactionsSuccessBody);
        var client = NewClient(handler);

        await client.Finance.QueryAccountTransactionsAsync(
            new QueryAccountTransactionsRequest
            {
                PageSize = 10,
                StartTime = "20180101",
                EndTime = "20180131",
                PageNum = 1,
            },
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("page_size=10", body);
        Assert.Contains("start_time=20180101", body);
        Assert.Contains("end_time=20180131", body);
        Assert.Contains("page_num=1", body);
        Assert.Contains("access_token=tok", body);
    }

    [Fact]
    public async Task QueryAccountTransactionsAsync_IncludesOptionalFilters()
    {
        var handler = new TestHandler(TransactionsSuccessBody);
        var client = NewClient(handler);

        await client.Finance.QueryAccountTransactionsAsync(
            new QueryAccountTransactionsRequest
            {
                TransactionType = "Order",
                SubTransactionType = "Sale",
                TransactionNumber = "TXN-001",
                PageSize = 10,
                StartTime = "20180101",
                EndTime = "20180131",
                PageNum = 1,
            },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("transaction_type=Order", body);
        Assert.Contains("sub_transaction_type=Sale", body);
        Assert.Contains("transaction_number=TXN-001", body);
    }

    [Fact]
    public async Task QueryAccountTransactionsAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"500","type":"ISV","message":"Invalid parameters","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Finance.QueryAccountTransactionsAsync(
            new QueryAccountTransactionsRequest { PageSize = 10, StartTime = "x", EndTime = "y", PageNum = 1 },
            "tok"));

        Assert.Equal("500", ex.ErrorCode);
    }

    [Fact]
    public async Task QueryAccountTransactionsAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(TransactionsSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Finance.QueryAccountTransactionsAsync(
            new QueryAccountTransactionsRequest { PageSize = 10, StartTime = "a", EndTime = "b", PageNum = 1 },
            ""));
    }

    [Fact]
    public async Task QueryAccountTransactionsAsync_RejectsZeroPageSize()
    {
        var client = NewClient(new TestHandler(TransactionsSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Finance.QueryAccountTransactionsAsync(
            new QueryAccountTransactionsRequest { PageSize = 0, StartTime = "a", EndTime = "b", PageNum = 1 },
            "tok"));
    }

    [Fact]
    public async Task QueryAccountTransactionsAsync_RejectsZeroPageNum()
    {
        var client = NewClient(new TestHandler(TransactionsSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Finance.QueryAccountTransactionsAsync(
            new QueryAccountTransactionsRequest { PageSize = 10, StartTime = "a", EndTime = "b", PageNum = 0 },
            "tok"));
    }

    // ─── QueryLogisticsFeeDetailAsync ───────────────────────────────────

    [Fact]
    public async Task QueryLogisticsFeeDetailAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(LogisticsFeeSuccessBody);
        var client = NewClient(handler);

        var response = await client.Finance.QueryLogisticsFeeDetailAsync(
            new QueryLogisticsFeeDetailRequest(),
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(50, response.Data.Total);
        Assert.Equal(1, response.Data.CurrentPage);
        Assert.NotNull(response.Data.List);
        var fee = Assert.Single(response.Data.List);
        Assert.Equal("ORD-001", fee.OrderNumber);
        Assert.Equal("DHL", fee.ShipmentProvider);
        Assert.Equal("5.00", fee.FeeAmount);
    }

    [Fact]
    public async Task QueryLogisticsFeeDetailAsync_BuildsGetUrl_WithOptionalParams()
    {
        var handler = new TestHandler(LogisticsFeeSuccessBody);
        var client = NewClient(handler);

        await client.Finance.QueryLogisticsFeeDetailAsync(
            new QueryLogisticsFeeDetailRequest
            {
                SellerId = "seller1",
                RequestType = "typeA",
                StartTime = "2018-01-01",
                EndTime = "2018-01-31",
                PageNum = 1,
                PageSize = 20,
            },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/lbs/slb/queryLogisticsFeeDetail?", url, StringComparison.Ordinal);
        Assert.Contains("seller_id=seller1", url, StringComparison.Ordinal);
        Assert.Contains("request_type=typeA", url, StringComparison.Ordinal);
        Assert.Contains("page_num=1", url, StringComparison.Ordinal);
        Assert.Contains("page_size=20", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task QueryLogisticsFeeDetailAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"403","type":"ISV","message":"Forbidden","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Finance.QueryLogisticsFeeDetailAsync(
            new QueryLogisticsFeeDetailRequest(),
            "tok"));

        Assert.Equal("403", ex.ErrorCode);
    }

    // ─── QueryTransactionDetailsAsync ───────────────────────────────────

    [Fact]
    public async Task QueryTransactionDetailsAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(TransactionDetailSuccessBody);
        var client = NewClient(handler);

        var response = await client.Finance.QueryTransactionDetailsAsync(
            new QueryTransactionDetailsRequest
            {
                StartDate = "2018-01-01",
                EndDate = "2018-01-31",
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(200, response.Data.Total);
        Assert.Equal(1, response.Data.CurrentPage);
        Assert.NotNull(response.Data.List);
        var td = Assert.Single(response.Data.List);
        Assert.Equal("TXN-D-001", td.TransactionNumber);
        Assert.Equal("Payout", td.TransactionType);
    }

    [Fact]
    public async Task QueryTransactionDetailsAsync_BuildsGetUrl_WithRequiredParams()
    {
        var handler = new TestHandler(TransactionDetailSuccessBody);
        var client = NewClient(handler);

        await client.Finance.QueryTransactionDetailsAsync(
            new QueryTransactionDetailsRequest
            {
                StartDate = "2018-01-01",
                EndDate = "2018-01-31",
            },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest.RequestUri!.ToString();
        Assert.StartsWith(UrlConstants.API_GATEWAY_URL_SG + "/finance/seller/transaction/detail?", url, StringComparison.Ordinal);
        Assert.Contains("start_date=2018-01-01", url, StringComparison.Ordinal);
        Assert.Contains("end_date=2018-01-31", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task QueryTransactionDetailsAsync_IncludesOptionalPaging()
    {
        var handler = new TestHandler(TransactionDetailSuccessBody);
        var client = NewClient(handler);

        await client.Finance.QueryTransactionDetailsAsync(
            new QueryTransactionDetailsRequest
            {
                StartDate = "2018-01-01",
                EndDate = "2018-01-31",
                PageNum = 2,
                PageSize = 50,
            },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("page_num=2", url, StringComparison.Ordinal);
        Assert.Contains("page_size=50", url, StringComparison.Ordinal);
    }

    [Fact]
    public async Task QueryTransactionDetailsAsync_Throws_OnErrorEnvelope()
    {
        const string ErrorBody = """{"code":"400","type":"ISV","message":"Bad request","request_id":"req-err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() => client.Finance.QueryTransactionDetailsAsync(
            new QueryTransactionDetailsRequest { StartDate = "x", EndDate = "y" },
            "tok"));

        Assert.Equal("400", ex.ErrorCode);
    }

    [Fact]
    public async Task QueryTransactionDetailsAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(TransactionDetailSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Finance.QueryTransactionDetailsAsync(
            new QueryTransactionDetailsRequest { StartDate = "a", EndDate = "b" },
            ""));
    }

    [Fact]
    public async Task QueryTransactionDetailsAsync_RejectsEmptyStartDate()
    {
        var client = NewClient(new TestHandler(TransactionDetailSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Finance.QueryTransactionDetailsAsync(
            new QueryTransactionDetailsRequest { StartDate = "", EndDate = "b" },
            "tok"));
    }

    [Fact]
    public async Task QueryTransactionDetailsAsync_RejectsEmptyEndDate()
    {
        var client = NewClient(new TestHandler(TransactionDetailSuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Finance.QueryTransactionDetailsAsync(
            new QueryTransactionDetailsRequest { StartDate = "a", EndDate = "" },
            "tok"));
    }
}
