using Laz.Sdk.Models.Logistics;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class LogisticsService(LazClient client) : ILogisticsService
{
    private readonly LazClient _client = client;

    public async Task<GetOrderTraceResponse> GetOrderTraceAsync(
        GetOrderTraceRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.OrderId);

        var lazRequest = new LazRequest("/logistic/order/trace") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("order_id", request.OrderId);
        if (!string.IsNullOrEmpty(request.Locale))
        {
            lazRequest.AddApiParameter("locale", request.Locale);
        }
        if (request.OfcPackageIdList is { Count: > 0 } ids)
        {
            lazRequest.AddApiParameter("ofcPackageIdList", "[" + string.Join(',', ids) + "]");
        }
        else
        {
            lazRequest.AddApiParameter("ofcPackageIdList", "[]");
        }

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetOrderTraceResponse>();
    }
}
