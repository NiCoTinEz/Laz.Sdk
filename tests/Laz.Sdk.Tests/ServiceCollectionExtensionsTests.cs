using Laz.Sdk.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Laz.Sdk.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddLazClient_Default_ResolvesILazClient()
    {
        var sp = new ServiceCollection()
            .AddLazClient(o => { o.AppKey = "k"; o.AppSecret = "s"; })
            .Services
            .BuildServiceProvider();

        var client = sp.GetRequiredService<ILazClient>();
        Assert.IsType<LazClient>(client);
    }

    [Fact]
    public void AddLazClient_Default_ExposesILazClientFactory()
    {
        var sp = new ServiceCollection()
            .AddLazClient(o => { o.AppKey = "k"; o.AppSecret = "s"; })
            .Services
            .BuildServiceProvider();

        var factory = sp.GetRequiredService<ILazClientFactory>();
        Assert.NotNull(factory);
    }

    [Fact]
    public void AddLazClient_Named_RegistersIndependentOptions()
    {
        var services = new ServiceCollection();
        services.AddLazClient("th", o => { o.AppKey = "kTH"; o.AppSecret = "sTH"; o.ServerUrl = UrlConstants.API_GATEWAY_URL_TH; });
        services.AddLazClient("id", o => { o.AppKey = "kID"; o.AppSecret = "sID"; o.ServerUrl = UrlConstants.API_GATEWAY_URL_ID; });
        var sp = services.BuildServiceProvider();

        var monitor = sp.GetRequiredService<IOptionsMonitor<LazClientOptions>>();
        Assert.Equal("kTH",                              monitor.Get("th").AppKey);
        Assert.Equal(UrlConstants.API_GATEWAY_URL_TH,    monitor.Get("th").ServerUrl);
        Assert.Equal("kID",                              monitor.Get("id").AppKey);
        Assert.Equal(UrlConstants.API_GATEWAY_URL_ID,    monitor.Get("id").ServerUrl);
    }

    [Fact]
    public void AddLazClient_Named_FactoryReturnsDistinctClients()
    {
        var services = new ServiceCollection();
        services.AddLazClient("th", o => { o.AppKey = "kTH"; o.AppSecret = "sTH"; });
        services.AddLazClient("id", o => { o.AppKey = "kID"; o.AppSecret = "sID"; });
        var sp = services.BuildServiceProvider();

        var factory = sp.GetRequiredService<ILazClientFactory>();
        var th = factory.CreateClient("th");
        var id = factory.CreateClient("id");

        Assert.NotSame(th, id);
    }

    [Fact]
    public void AddLazClient_Named_DoesNotRegisterDefault()
    {
        var services = new ServiceCollection();
        services.AddLazClient("th", o => { o.AppKey = "kTH"; o.AppSecret = "sTH"; });
        var sp = services.BuildServiceProvider();

        Assert.Null(sp.GetService<ILazClient>());
    }

    [Fact]
    public void AddLazClient_FromConfiguration_BindsOptions()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AppKey"]    = "k",
                ["AppSecret"] = "s",
                ["ServerUrl"] = UrlConstants.API_GATEWAY_URL_VN,
            })
            .Build();

        var sp = new ServiceCollection()
            .AddLazClient(config)
            .Services
            .BuildServiceProvider();

        var opts = sp.GetRequiredService<IOptions<LazClientOptions>>().Value;
        Assert.Equal("k",                            opts.AppKey);
        Assert.Equal("s",                            opts.AppSecret);
        Assert.Equal(UrlConstants.API_GATEWAY_URL_VN, opts.ServerUrl);
    }

    [Fact]
    public void AddLazClient_RegistersValidation()
    {
        var sp = new ServiceCollection()
            .AddLazClient(o =>
            {
                // intentionally leaving AppKey/AppSecret blank — required validation should fire on resolve
                o.AppKey    = "";
                o.AppSecret = "";
            })
            .Services
            .BuildServiceProvider();

        Assert.Throws<OptionsValidationException>(() =>
            sp.GetRequiredService<IOptions<LazClientOptions>>().Value);
    }
}
