using Laz.Sdk.Models.Fulfillment;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class ReadyToShipV2Tests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """
    {
      "result":{
        "error_msg":"package not found",
        "data":{
          "packages":[
            {"msg":"package already cancelled","item_err_code":"600002","package_id":"FP038524014","retry":"false"}
          ]
        },
        "success":"true",
        "error_code":"11"
      },
      "code":"0",
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task ReadyToShipV2Async_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Fulfillment.ReadyToShipV2Async(
            new ReadyToShipV2Request { Packages = new[] { new AwbPackage { PackageId = "FP234234" } } },
            "tok");

        Assert.True(response.Result!.Success);
        var pkg = Assert.Single(response.Result.Data!.Packages!);
        Assert.Equal("FP038524014",                pkg.PackageId);
        Assert.Equal("package already cancelled",  pkg.Msg);
        Assert.Equal(600002,                       pkg.ItemErrCode);
        Assert.False(pkg.Retry);
    }

    [Fact]
    public async Task ReadyToShipV2Async_BuildsReadyToShipReq_Body()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Fulfillment.ReadyToShipV2Async(
            new ReadyToShipV2Request
            {
                Packages = new[]
                {
                    new AwbPackage { PackageId = "FP1" },
                    new AwbPackage { PackageId = "FP2" },
                },
            },
            "tok");

        var body = handler.LastRequestBody!;
        var i = body.IndexOf("readyToShipReq=", StringComparison.Ordinal) + "readyToShipReq=".Length;
        var raw = body[i..];
        var amp = raw.IndexOf('&');
        if (amp >= 0) raw = raw[..amp];
        var decoded = Uri.UnescapeDataString(raw);

        Assert.Contains("\"package_id\":\"FP1\"", decoded, StringComparison.Ordinal);
        Assert.Contains("\"package_id\":\"FP2\"", decoded, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ReadyToShipV2Async_RejectsEmptyPackages()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Fulfillment.ReadyToShipV2Async(
                new ReadyToShipV2Request { Packages = Array.Empty<AwbPackage>() },
                "tok"));
    }
}
