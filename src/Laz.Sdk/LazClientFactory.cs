using Microsoft.Extensions.Options;

namespace Laz.Sdk;

internal sealed class LazClientFactory(
    IHttpClientFactory httpClientFactory,
    IOptionsMonitor<LazClientOptions> optionsMonitor) : ILazClientFactory
{
    public ILazClient CreateClient(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        var http = httpClientFactory.CreateClient($"Laz.Sdk:{name}");
        var opts = optionsMonitor.Get(name);
        return new LazClient(http, opts);
    }
}
