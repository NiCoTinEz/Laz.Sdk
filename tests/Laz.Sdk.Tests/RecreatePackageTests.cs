using Laz.Sdk.Models.Fulfillment;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class RecreatePackageTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """
    {
      "result":{
        "error_msg":"package not found",
        "data":{
          "packages":[{"msg":"package not found","item_err_code":"600001","package_id":"FP12312321","retry":"true"}]
        },
        "success":"false",
        "error_code":"2342"
      },
      "code":"0",
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task RecreatePackageAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Fulfillment.RecreatePackageAsync(
            new RecreatePackageRequest { Packages = new[] { new AwbPackage { PackageId = "FP2342342" } } },
            "tok");

        Assert.False(response.Result!.Success);
        var pkg = Assert.Single(response.Result.Data!.Packages!);
        Assert.Equal("FP12312321",       pkg.PackageId);
        Assert.Equal(600001,             pkg.ItemErrCode);
        Assert.True(pkg.Retry);
    }

    [Fact]
    public async Task RecreatePackageAsync_BuildsRePackReq_Body()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Fulfillment.RecreatePackageAsync(
            new RecreatePackageRequest { Packages = new[] { new AwbPackage { PackageId = "FP1" } } },
            "tok");

        var body = handler.LastRequestBody!;
        Assert.Contains("rePackReq=", body, StringComparison.Ordinal);
        var i = body.IndexOf("rePackReq=", StringComparison.Ordinal) + "rePackReq=".Length;
        var raw = body[i..];
        var amp = raw.IndexOf('&');
        if (amp >= 0) raw = raw[..amp];
        Assert.Contains("\"package_id\":\"FP1\"", Uri.UnescapeDataString(raw), StringComparison.Ordinal);
    }

    [Fact]
    public async Task RecreatePackageAsync_RejectsEmptyPackages()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Fulfillment.RecreatePackageAsync(
                new RecreatePackageRequest { Packages = Array.Empty<AwbPackage>() },
                "tok"));
    }
}
