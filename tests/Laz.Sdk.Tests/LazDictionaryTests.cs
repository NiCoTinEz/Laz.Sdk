using System.Globalization;
using Xunit;

namespace Laz.Sdk.Tests;

public class LazDictionaryTests
{
    [Fact]
    public void Add_String_StoresValue()
    {
        var dict = new LazDictionary();
        dict.Add("k", "v");
        Assert.Equal("v", dict["k"]);
    }

    [Fact]
    public void Add_DropsEmptyKey()
    {
        var dict = new LazDictionary();
        dict.Add("", "v");
        Assert.Empty(dict);
    }

    [Fact]
    public void Add_DropsNullOrEmptyValue()
    {
        var dict = new LazDictionary();
        dict.Add("k", (string?)null);
        dict.Add("k", "");
        Assert.Empty(dict);
    }

    [Fact]
    public void Add_Int_UsesInvariantCulture()
    {
        var prior = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo("th-TH"); // non-English grouping/format
        try
        {
            var dict = new LazDictionary();
            dict.Add("n", (object)1234567);
            Assert.Equal("1234567", dict["n"]);
        }
        finally
        {
            CultureInfo.CurrentCulture = prior;
        }
    }

    [Fact]
    public void Add_Double_UsesInvariantDecimalPoint()
    {
        var prior = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo("de-DE"); // comma-decimal locale
        try
        {
            var dict = new LazDictionary();
            dict.Add("d", (object)1.5);
            Assert.Equal("1.5", dict["d"]);
        }
        finally
        {
            CultureInfo.CurrentCulture = prior;
        }
    }

    [Fact]
    public void Add_Bool_UsesLowercase()
    {
        var dict = new LazDictionary();
        dict.Add("t", (object)true);
        dict.Add("f", (object)false);
        Assert.Equal("true", dict["t"]);
        Assert.Equal("false", dict["f"]);
    }

    [Fact]
    public void Add_DateTimeOffset_UsesUnixMilliseconds()
    {
        var dict = new LazDictionary();
        var dto = DateTimeOffset.FromUnixTimeMilliseconds(1700000000000);
        dict.Add("ts", (object)dto);
        Assert.Equal("1700000000000", dict["ts"]);
    }

    [Fact]
    public void AddAll_MergesSource()
    {
        var dict = new LazDictionary();
        dict.AddAll(new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["a"] = "1",
            ["b"] = "2",
        });
        Assert.Equal(2, dict.Count);
        Assert.Equal("1", dict["a"]);
        Assert.Equal("2", dict["b"]);
    }
}
