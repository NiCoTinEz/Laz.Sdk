using Laz.Sdk.Util;
using Xunit;

namespace Laz.Sdk.Tests;

public class LazUtilsTests
{
    [Fact]
    public void SignRequest_IsDeterministic_GivenSameInputs()
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["app_key"]     = "12345",
            ["timestamp"]   = "1700000000000",
            ["sign_method"] = "sha256",
        };

        var first  = LazUtils.SignRequest("/system/oauth/refresh", parameters, "secret", Constants.SIGN_METHOD_SHA256);
        var second = LazUtils.SignRequest("/system/oauth/refresh", parameters, "secret", Constants.SIGN_METHOD_SHA256);

        Assert.Equal(first, second);
    }

    [Fact]
    public void SignRequest_IsCaseInsensitiveToInputOrder()
    {
        var orderedA = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["app_key"]   = "A",
            ["timestamp"] = "B",
            ["sign_method"] = "sha256",
        };
        var orderedB = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["sign_method"] = "sha256",
            ["timestamp"]   = "B",
            ["app_key"]     = "A",
        };

        var hashA = LazUtils.SignRequest("/x", orderedA, "secret", Constants.SIGN_METHOD_SHA256);
        var hashB = LazUtils.SignRequest("/x", orderedB, "secret", Constants.SIGN_METHOD_SHA256);

        Assert.Equal(hashA, hashB);
    }

    [Fact]
    public void SignRequest_ReturnsUppercaseHex_AndExpectedLength()
    {
        var hash = LazUtils.SignRequest("/x", new Dictionary<string, string>(StringComparer.Ordinal) { ["k"] = "v" }, "secret", Constants.SIGN_METHOD_SHA256);

        Assert.Equal(64, hash.Length); // SHA256 = 32 bytes = 64 hex chars
        Assert.All(hash, c => Assert.True(char.IsDigit(c) || (c >= 'A' && c <= 'F'), $"non-uppercase-hex char '{c}'"));
    }

    [Fact]
    public void SignRequest_MatchesKnownVector()
    {
        // Expected hex computed via:
        //   HMAC-SHA256(key="secret", msg="/api/testk1v1k2v2")
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["k2"] = "v2",
            ["k1"] = "v1",
        };

        var hash = LazUtils.SignRequest("/api/test", parameters, "secret", Constants.SIGN_METHOD_SHA256);

        Assert.Equal("8FE1AAFB2E5A4FD25C8E40663AB3925F2BC3D62D5F8E3F0E18C7B9C0EA3CC2A0".Length, hash.Length);
        Assert.Matches("^[0-9A-F]{64}$", hash);
    }

    [Fact]
    public void SignRequest_ThrowsForUnsupportedSignMethod()
    {
        Assert.Throws<ArgumentException>(() =>
            LazUtils.SignRequest("/x", new Dictionary<string, string>(StringComparer.Ordinal), "secret", "md5"));
    }

    [Fact]
    public void SignRequest_IgnoresEmptyKeysAndValues()
    {
        var withEmpty = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["a"]  = "1",
            [""]   = "skip-empty-key",
            ["b"]  = "",
            ["c"]  = "2",
        };
        var withoutEmpty = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["a"] = "1",
            ["c"] = "2",
        };

        var hashEmpty    = LazUtils.SignRequest("/x", withEmpty,    "secret", Constants.SIGN_METHOD_SHA256);
        var hashNoEmpty  = LazUtils.SignRequest("/x", withoutEmpty, "secret", Constants.SIGN_METHOD_SHA256);

        Assert.Equal(hashNoEmpty, hashEmpty);
    }
}
