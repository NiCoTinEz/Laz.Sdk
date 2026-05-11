namespace Laz.Sdk.Services;

internal sealed class LogisticsService(LazClient client) : ILogisticsService
{
    private readonly LazClient _client = client;
}
