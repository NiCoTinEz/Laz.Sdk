using Xunit;

namespace Laz.Sdk.Tests;

public class LazResponseTests
{
    [Fact]
    public void IsError_True_WhenCodeNonZero()
    {
        var r = new LazResponse { Code = "ApiCallLimit" };
        Assert.True(r.IsError());
    }

    [Fact]
    public void IsError_False_WhenCodeZero()
    {
        var r = new LazResponse { Code = "0" };
        Assert.False(r.IsError());
    }

    [Fact]
    public void IsError_False_WhenCodeMissing()
    {
        Assert.False(new LazResponse().IsError());
        Assert.False(new LazResponse { Code = "" }.IsError());
    }
}
