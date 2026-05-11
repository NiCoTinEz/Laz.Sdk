using System.Text.Json;
using Laz.Sdk.Models.Fulfillment;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class FulfillmentService(LazClient client) : IFulfillmentService
{
    private readonly LazClient _client = client;

    public async Task<GetShipmentProvidersResponse> GetShipmentProvidersAsync(
        GetShipmentProvidersRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (request.Orders is null || request.Orders.Count == 0)
        {
            throw new ArgumentException("At least one order is required.", nameof(request));
        }

        // Lazada wants the order_item_ids inner arrays as JSON-string literals
        // (e.g. [ "[2342342,23423]" ]) — not as native JSON arrays. Format manually.
        var serialized = SerializeShipmentProvidersReq(request);

        var lazRequest = new LazRequest("/order/shipment/providers/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("getShipmentProvidersReq", serialized);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetShipmentProvidersResponse>();
    }

    private static string SerializeShipmentProvidersReq(GetShipmentProvidersRequest request)
    {
        var ordersJson = request.Orders.Select(o => new
        {
            order_id = o.OrderId,
            order_item_ids = o.OrderItemIds
                .Select(group => "[" + string.Join(',', group) + "]")
                .ToArray(),
        });

        return JsonSerializer.Serialize(new { orders = ordersJson });
    }
}
