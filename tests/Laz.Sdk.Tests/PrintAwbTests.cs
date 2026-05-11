using Laz.Sdk.Models.Fulfillment;
using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class PrintAwbTests
{
    private static LazClient NewClient(TestHandler handler)
        => new(new HttpClient(handler), new LazClientOptions { AppKey = "ak", AppSecret = "as" });

    private const string SuccessBody = """
    {
      "result":{
        "error_msg":"package not found",
        "data":{
          "file":"PGlmcmFtZSBzcm",
          "pdf_url":"http://www.test.com/xxx.pdf",
          "doc_type":"PDF"
        },
        "success":"true",
        "error_code":"123"
      },
      "code":"0",
      "request_id":"r"
    }
    """;

    [Fact]
    public async Task PrintAwbAsync_ParsesSample()
    {
        var client = NewClient(new TestHandler(SuccessBody));

        var response = await client.Fulfillment.PrintAwbAsync(
            new PrintAwbRequest
            {
                Packages = new[] { new AwbPackage { PackageId = "FP234234" } },
            },
            "tok");

        Assert.Equal("0", response.Code);
        Assert.True(response.Result!.Success);
        Assert.Equal("PDF",                            response.Result.Data!.DocType);
        Assert.Equal("http://www.test.com/xxx.pdf",    response.Result.Data.PdfUrl);
        Assert.Equal("PGlmcmFtZSBzcm",                 response.Result.Data.File);
    }

    [Fact]
    public async Task PrintAwbAsync_BuildsGetDocumentReq_Body()
    {
        var handler = new TestHandler(SuccessBody);
        var client = NewClient(handler);

        await client.Fulfillment.PrintAwbAsync(
            new PrintAwbRequest
            {
                DocType       = "PDF",
                PrintItemList = false,
                Packages      = new[]
                {
                    new AwbPackage { PackageId = "FP234234" },
                    new AwbPackage { PackageId = "FP999999" },
                },
            },
            "tok");

        var url = handler.LastRequest!.RequestUri!.ToString();
        var i = url.IndexOf("getDocumentReq=", StringComparison.Ordinal);
        Assert.True(i >= 0);
        var raw = url[(i + "getDocumentReq=".Length)..];
        var amp = raw.IndexOf('&');
        if (amp >= 0) raw = raw[..amp];
        var decoded = Uri.UnescapeDataString(raw);

        Assert.Contains("\"doc_type\":\"PDF\"",          decoded, StringComparison.Ordinal);
        Assert.Contains("\"print_item_list\":\"false\"", decoded, StringComparison.Ordinal);
        Assert.Contains("\"package_id\":\"FP234234\"",   decoded, StringComparison.Ordinal);
        Assert.Contains("\"package_id\":\"FP999999\"",   decoded, StringComparison.Ordinal);
    }

    [Fact]
    public async Task PrintAwbAsync_RejectsEmptyPackages()
    {
        var client = NewClient(new TestHandler(SuccessBody));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Fulfillment.PrintAwbAsync(
                new PrintAwbRequest { Packages = Array.Empty<AwbPackage>() },
                "tok"));
    }

    [Fact]
    public void PrintAwbData_GetFileBytes_RoundTrips()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes("hello");
        var b64 = Convert.ToBase64String(bytes);
        var data = new PrintAwbData { File = b64, DocType = "PDF" };
        Assert.Equal(bytes, data.GetFileBytes());

        Assert.Empty(new PrintAwbData().GetFileBytes());
    }
}
