using Laz.Sdk.Models.Media;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class MediaServiceTests
{
    private static LazClient NewClient(TestHandler handler)
    {
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as" };
        var http = new HttpClient(handler);
        return new LazClient(http, opts);
    }

    private const string InitCreateBody = """
    {
      "code":"0",
      "data":{
        "upload_id":"upload-abc-123",
        "success":true,
        "result_code":"0",
        "result_message":"Success"
      },
      "request_id":"req-init-1"
    }
    """;

    private const string UploadBlockBody = """
    {
      "code":"0",
      "data":{
        "e_tag":"etag-001",
        "success":true,
        "result_code":"0",
        "result_message":"Success"
      },
      "request_id":"req-upload-1"
    }
    """;

    private const string CompleteCreateBody = """
    {
      "code":"0",
      "data":{
        "video_id":"12345",
        "success":true,
        "result_code":"0",
        "result_message":"Success"
      },
      "request_id":"req-complete-1"
    }
    """;

    private const string GetVideoBody = """
    {
      "code":"0",
      "data":{
        "video_id":"12345",
        "cover_url":"https://example.com/cover.jpg",
        "video_url":"https://example.com/video.mp4",
        "state":"active",
        "title":"My Video",
        "duration":"120",
        "file_size":"1048576",
        "created_time":"2026-01-15 10:00:00"
      },
      "request_id":"req-get-1"
    }
    """;

    private const string RemoveVideoBody = """
    {
      "code":"0",
      "data":{
        "success":true,
        "result_code":"0",
        "result_message":"Success"
      },
      "request_id":"req-remove-1"
    }
    """;

    private const string VideoQuotaBody = """
    {
      "code":"0",
      "data":{
        "total_quota":"100",
        "used_quota":"10",
        "remaining_quota":"90"
      },
      "request_id":"req-quota-1"
    }
    """;

    private const string UploadImageBody = """
    {
      "code":"0",
      "data":{
        "image_url":"https://example.com/image.jpg",
        "hash_code":"hash123"
      },
      "request_id":"req-img-1"
    }
    """;

    // ──────────────────────────────────────────────
    // InitCreateVideoAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task InitCreateVideoAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(InitCreateBody);
        var client = NewClient(handler);

        var response = await client.Media.InitCreateVideoAsync("test.mp4", 1048576, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("upload-abc-123", response.Data!.UploadId);
        Assert.True(response.Data.Success);
    }

    [Fact]
    public async Task InitCreateVideoAsync_BuildsPostRequest()
    {
        var handler = new TestHandler(InitCreateBody);
        var client = NewClient(handler);

        await client.Media.InitCreateVideoAsync("test.mp4", 1048576, "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = handler.LastRequestBody!;
        Assert.Contains("file_name=test.mp4", body);
        Assert.Contains("file_bytes=1048576", body);
    }

    [Fact]
    public async Task InitCreateVideoAsync_RejectsEmptyFileName()
    {
        var client = NewClient(new TestHandler(InitCreateBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Media.InitCreateVideoAsync("", 100, "tok"));
    }

    [Fact]
    public async Task InitCreateVideoAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(InitCreateBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Media.InitCreateVideoAsync("test.mp4", 100, ""));
    }

    // ──────────────────────────────────────────────
    // UploadVideoBlockAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task UploadVideoBlockAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(UploadBlockBody);
        var client = NewClient(handler);

        var file = new FileItem("block.bin", new byte[] { 0x00, 0x01, 0x02 });
        var response = await client.Media.UploadVideoBlockAsync("upload-abc-123", 0, 3, file, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("etag-001", response.Data!.ETag);
        Assert.True(response.Data.Success);
    }

    [Fact]
    public async Task UploadVideoBlockAsync_BuildsMultipartRequest()
    {
        var handler = new TestHandler(UploadBlockBody);
        var client = NewClient(handler);

        var file = new FileItem("block.bin", new byte[] { 0x00, 0x01, 0x02 });
        await client.Media.UploadVideoBlockAsync("upload-abc-123", 0, 3, file, "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        // With file params, the request uses multipart content
        Assert.NotNull(handler.LastRequest!.Content);
        Assert.IsType<MultipartFormDataContent>(handler.LastRequest.Content);
    }

    [Fact]
    public async Task UploadVideoBlockAsync_RejectsEmptyUploadId()
    {
        var client = NewClient(new TestHandler(UploadBlockBody));
        var file = new FileItem("block.bin", new byte[] { 0x00 });
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Media.UploadVideoBlockAsync("", 0, 1, file, "tok"));
    }

    // ──────────────────────────────────────────────
    // CompleteCreateVideoAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task CompleteCreateVideoAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(CompleteCreateBody);
        var client = NewClient(handler);

        var response = await client.Media.CompleteCreateVideoAsync(
            "upload-abc-123",
            "[{\"partNumber\":1,\"eTag\":\"etag-001\"}]",
            "My Video",
            accessToken: "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(12345, response.Data!.VideoId);
        Assert.True(response.Data.Success);
    }

    [Fact]
    public async Task CompleteCreateVideoAsync_SendsAllParams()
    {
        var handler = new TestHandler(CompleteCreateBody);
        var client = NewClient(handler);

        await client.Media.CompleteCreateVideoAsync(
            "upload-abc-123",
            "[{\"partNumber\":1,\"eTag\":\"etag-001\"}]",
            "My Video",
            coverUrl: "https://example.com/cover.jpg",
            videoUsage: "product",
            accessToken: "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("upload_id=upload-abc-123", body);
        Assert.Contains("parts=" + Uri.EscapeDataString("[{\"partNumber\":1,\"eTag\":\"etag-001\"}]"), body);
        Assert.Contains("title=My+Video", body);
        Assert.Contains("cover_url=https%3A%2F%2Fexample.com%2Fcover.jpg", body);
        Assert.Contains("video_usage=product", body);
    }

    [Fact]
    public async Task CompleteCreateVideoAsync_OmitsOptionalParams_WhenNull()
    {
        var handler = new TestHandler(CompleteCreateBody);
        var client = NewClient(handler);

        await client.Media.CompleteCreateVideoAsync(
            "upload-abc-123",
            "[{\"partNumber\":1,\"eTag\":\"etag-001\"}]",
            "My Video",
            accessToken: "tok");

        var body = handler.LastRequestBody!;
        Assert.DoesNotContain("cover_url", body);
        Assert.DoesNotContain("video_usage", body);
    }

    // ──────────────────────────────────────────────
    // GetVideoAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetVideoAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(GetVideoBody);
        var client = NewClient(handler);

        var response = await client.Media.GetVideoAsync(12345, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(12345, response.Data!.VideoId);
        Assert.Equal("https://example.com/cover.jpg", response.Data.CoverUrl);
        Assert.Equal("https://example.com/video.mp4", response.Data.VideoUrl);
        Assert.Equal("active", response.Data.State);
        Assert.Equal("My Video", response.Data.Title);
    }

    [Fact]
    public async Task GetVideoAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(GetVideoBody);
        var client = NewClient(handler);

        await client.Media.GetVideoAsync(12345, "tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("video_id=12345", url);
    }

    [Fact]
    public async Task GetVideoAsync_RejectsEmptyAccessToken()
    {
        var client = NewClient(new TestHandler(GetVideoBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Media.GetVideoAsync(12345, ""));
    }

    // ──────────────────────────────────────────────
    // RemoveVideoAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task RemoveVideoAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(RemoveVideoBody);
        var client = NewClient(handler);

        var response = await client.Media.RemoveVideoAsync(12345, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.True(response.Data!.Success);
    }

    [Fact]
    public async Task RemoveVideoAsync_SendsVideoId()
    {
        var handler = new TestHandler(RemoveVideoBody);
        var client = NewClient(handler);

        await client.Media.RemoveVideoAsync(12345, "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("video_id=12345", body);
    }

    // ──────────────────────────────────────────────
    // GetVideoQuotaAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetVideoQuotaAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(VideoQuotaBody);
        var client = NewClient(handler);

        var response = await client.Media.GetVideoQuotaAsync("tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal(100, response.Data!.TotalQuota);
        Assert.Equal(10, response.Data.UsedQuota);
        Assert.Equal(90, response.Data.RemainingQuota);
    }

    [Fact]
    public async Task GetVideoQuotaAsync_BuildsGetUrl()
    {
        var handler = new TestHandler(VideoQuotaBody);
        var client = NewClient(handler);

        await client.Media.GetVideoQuotaAsync("tok");

        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("/media/video/quota/get", handler.LastRequest!.RequestUri!.ToString());
    }

    // ──────────────────────────────────────────────
    // UploadImageAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task UploadImageAsync_ParsesSuccessResponse()
    {
        var handler = new TestHandler(UploadImageBody);
        var client = NewClient(handler);

        var image = new FileItem("photo.jpg", new byte[] { 0xFF, 0xD8, 0xFF });
        var response = await client.Media.UploadImageAsync(image, "tok");

        Assert.Equal("0", response.Code);
        Assert.NotNull(response.Data);
        Assert.Equal("https://example.com/image.jpg", response.Data!.ImageUrl);
        Assert.Equal("hash123", response.Data.HashCode);
    }

    [Fact]
    public async Task UploadImageAsync_BuildsMultipartRequest()
    {
        var handler = new TestHandler(UploadImageBody);
        var client = NewClient(handler);

        var image = new FileItem("photo.jpg", new byte[] { 0xFF, 0xD8, 0xFF });
        await client.Media.UploadImageAsync(image, "tok");

        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.NotNull(handler.LastRequest!.Content);
        Assert.IsType<MultipartFormDataContent>(handler.LastRequest.Content);
    }

    [Fact]
    public async Task UploadImageAsync_RejectsNullImage()
    {
        var client = NewClient(new TestHandler(UploadImageBody));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.Media.UploadImageAsync(null!, "tok"));
    }

    // ──────────────────────────────────────────────
    // Error handling
    // ──────────────────────────────────────────────

    [Fact]
    public async Task InitCreateVideoAsync_ThrowsOnError()
    {
        const string ErrorBody = """{"code":"1001","type":"ISV","message":"Invalid file","request_id":"err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Media.InitCreateVideoAsync("test.mp4", 100, "tok"));

        Assert.Equal("1001", ex.ErrorCode);
    }

    [Fact]
    public async Task GetVideoAsync_ThrowsOnError()
    {
        const string ErrorBody = """{"code":"32","type":"ISV","message":"Video not found","request_id":"err"}""";
        var client = NewClient(new TestHandler(ErrorBody));

        var ex = await Assert.ThrowsAsync<LazException>(() =>
            client.Media.GetVideoAsync(99999, "tok"));

        Assert.Equal("32", ex.ErrorCode);
    }

    // ──────────────────────────────────────────────
    // Regional gateway
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetVideoAsync_RespectsRegionalGateway()
    {
        var handler = new TestHandler(GetVideoBody);
        var opts = new LazClientOptions { AppKey = "ak", AppSecret = "as", ServerUrl = UrlConstants.API_GATEWAY_URL_TH };
        var http = new HttpClient(handler);
        var client = new LazClient(http, opts);

        await client.Media.GetVideoAsync(12345, "tok");

        Assert.StartsWith(
            UrlConstants.API_GATEWAY_URL_TH + "/media/video/get?",
            handler.LastRequest!.RequestUri!.ToString(),
            StringComparison.Ordinal);
    }
}
