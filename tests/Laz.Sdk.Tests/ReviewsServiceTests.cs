using Laz.Sdk.Models.Reviews;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class ReviewsServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string HistoryListBody = """
    {
      "code":"0",
      "data":{
        "id_list":[1001,1002,1003],
        "current":"1",
        "total":"3",
        "page_size":"10"
      },
      "request_id":"req-hist-1"
    }
    """;

    private const string ReviewListBody = """
    {
      "code":"0",
      "data":[
        {
          "id":"1001",
          "item_id":"2001",
          "order_id":"3001",
          "sku_id":"4001",
          "seller_sku":"SKU-001",
          "buyer_name":"John",
          "title":"Great product",
          "content":"Really liked it",
          "rating":5,
          "rating_1":0,
          "rating_2":0,
          "rating_3":0,
          "rating_4":0,
          "rating_5":1,
          "images":["https://example.com/img.jpg"],
          "videos":[{"url":"https://example.com/vid.mp4","cover":"https://example.com/cover.jpg"}],
          "created_time":"2026-01-15 10:00:00",
          "updated_time":"2026-01-15 12:00:00",
          "status":"published",
          "reply":"Thank you!"
        }
      ],
      "request_id":"req-list-1"
    }
    """;

    private const string SubmitReplyBody = """
    {
      "code":"0",
      "data":{
        "success":true,
        "result_code":"0",
        "result_message":"Success"
      },
      "request_id":"req-reply-1"
    }
    """;

    // ──────────────────────────────────────────────
    // GetHistoryReviewIdListAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetHistoryReviewIdListAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(HistoryListBody);
        var client = NewClient(handler);

        var response = await client.Reviews.GetHistoryReviewIdListAsync(
            itemId: 2001,
            startTime: 1700000000000,
            endTime: 1700100000000,
            current: 1,
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data!.IdList);
        Assert.Equal(3, response.Data.IdList!.Count);
        Assert.Equal(1001, response.Data.IdList[0]);
        Assert.Equal(1003, response.Data.IdList[2]);
        Assert.Equal(1, response.Data.Current);
        Assert.Equal(3, response.Data.Total);
    }

    [Fact]
    public async Task GetHistoryReviewIdListAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(HistoryListBody);
        var client = NewClient(handler);

        await client.Reviews.GetHistoryReviewIdListAsync(
            itemId: 2001,
            startTime: 1700000000000,
            endTime: 1700100000000,
            current: 1,
            accessToken: "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("item_id=2001", url);
        Assert.Contains("start_time=1700000000000", url);
        Assert.Contains("end_time=1700100000000", url);
        Assert.Contains("current=1", url);
    }

    [Fact]
    public async Task GetHistoryReviewIdListAsync_SendsOptionalOrderId()
    {
        var handler = new TestHandler(HistoryListBody);
        var client = NewClient(handler);

        await client.Reviews.GetHistoryReviewIdListAsync(
            itemId: 2001,
            startTime: 1700000000000,
            endTime: 1700100000000,
            current: 1,
            orderId: 5001,
            accessToken: "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("order_id=5001", url);
    }

    [Fact]
    public async Task GetHistoryReviewIdListAsync_OmitsOrderId_WhenNull()
    {
        var handler = new TestHandler(HistoryListBody);
        var client = NewClient(handler);

        await client.Reviews.GetHistoryReviewIdListAsync(
            itemId: 2001,
            startTime: 1700000000000,
            endTime: 1700100000000,
            current: 1,
            accessToken: "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.DoesNotContain("order_id", url);
    }

    // ──────────────────────────────────────────────
    // GetReviewListByIdListAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetReviewListByIdListAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(ReviewListBody);
        var client = NewClient(handler);

        var response = await client.Reviews.GetReviewListByIdListAsync(
            new long[] { 1001 },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data!);
        Assert.Equal(1001, response.Data[0].Id);
        Assert.Equal(2001, response.Data[0].ItemId);
        Assert.Equal("Great product", response.Data[0].Title);
        Assert.Equal("Really liked it", response.Data[0].Content);
        Assert.Equal(5, response.Data[0].Rating);
        Assert.NotNull(response.Data[0].Images);
        Assert.Single(response.Data[0].Images!);
        Assert.NotNull(response.Data[0].Videos);
        Assert.Single(response.Data[0].Videos!);
        Assert.Equal("https://example.com/vid.mp4", response.Data[0].Videos![0].Url);
        Assert.Equal("Thank you!", response.Data[0].Reply);
    }

    [Fact]
    public async Task GetReviewListByIdListAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(ReviewListBody);
        var client = NewClient(handler);

        await client.Reviews.GetReviewListByIdListAsync(
            new long[] { 1001, 1002 },
            "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("id_list=%5B1001%2C1002%5D", url); // URL-encoded [1001,1002]
    }

    [Fact]
    public async Task GetReviewListByIdListAsync_RejectsEmptyIdList()
    {
        var client = NewClient(new TestHandler(ReviewListBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Reviews.GetReviewListByIdListAsync(Array.Empty<long>(), "tok"));
    }

    [Fact]
    public async Task GetReviewListByIdListAsync_RejectsOver10Ids()
    {
        var client = NewClient(new TestHandler(ReviewListBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Reviews.GetReviewListByIdListAsync(new long[11], "tok"));
    }

    [Fact]
    public async Task GetReviewListByIdListAsync_RejectsNullIdList()
    {
        var client = NewClient(new TestHandler(ReviewListBody));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.Reviews.GetReviewListByIdListAsync(null!, "tok"));
    }

    // ──────────────────────────────────────────────
    // SubmitSellerReplyAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SubmitSellerReplyAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(SubmitReplyBody);
        var client = NewClient(handler);

        var response = await client.Reviews.SubmitSellerReplyAsync(
            1001,
            "Thank you for your feedback!",
            "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    [Fact]
    public async Task SubmitSellerReplyAsync_SendsPostWithFormData()
    {
        var handler = new TestHandler(SubmitReplyBody);
        var client = NewClient(handler);

        await client.Reviews.SubmitSellerReplyAsync(
            1001,
            "Thank you for your feedback!",
            "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("review_id=1001", body);
        Assert.Contains("content=", body);
        Assert.Contains("feedback", body);
    }

    [Fact]
    public async Task SubmitSellerReplyAsync_RejectsEmptyContent()
    {
        var client = NewClient(new TestHandler(SubmitReplyBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Reviews.SubmitSellerReplyAsync(1001, "", "tok"));
    }

    // ──────────────────────────────────────────────
    // Error handling
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetHistoryReviewIdListAsync_ThrowsOnError()
    {
        const string ErrorBody = """{"code":"1001","type":"ISV","message":"Invalid item","request_id":"err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Reviews.GetHistoryReviewIdListAsync(2001, 1700000000000, 1700100000000, 1, accessToken: "tok"));

        Assert.Equal("1001", ex.ErrorCode);
    }

    // ──────────────────────────────────────────────
    // Regional gateway
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetHistoryReviewIdListAsync_RespectsRegionalGateway()
    {
        var handler = new TestHandler(HistoryListBody);
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as", ServerUrl = UrlConstants.API_GATEWAY_URL_TH };
        var http = new HttpClient(handler);
        var client = new LazClient(http, opts);

        await client.Reviews.GetHistoryReviewIdListAsync(2001, 1700000000000, 1700100000000, 1, accessToken: "tok");

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_TH + "/review/seller/history/list?",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }
}
