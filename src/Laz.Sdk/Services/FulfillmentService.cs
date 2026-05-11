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

    public async Task<PrintAwbResponse> PrintAwbAsync(
        PrintAwbRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.DocType);
        if (request.Packages is null || request.Packages.Count == 0)
        {
            throw new ArgumentException("At least one package is required.", nameof(request));
        }

        var payload = JsonSerializer.Serialize(new
        {
            doc_type        = request.DocType,
            print_item_list = request.PrintItemList ? "true" : "false",
            packages        = request.Packages.Select(p => new { package_id = p.PackageId }).ToArray(),
        });

        var lazRequest = new LazRequest("/order/package/document/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("getDocumentReq", payload);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<PrintAwbResponse>();
    }

    public async Task<PackV2Response> PackV2Async(
        PackV2Request request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.DeliveryType);
        ArgumentException.ThrowIfNullOrEmpty(request.ShipmentProviderCode);
        if (request.PackOrderList is null || request.PackOrderList.Count == 0)
        {
            throw new ArgumentException("PackOrderList must contain at least one order.", nameof(request));
        }

        var payload = JsonSerializer.Serialize(new
        {
            pack_order_list = request.PackOrderList.Select(o => new
            {
                order_item_list = o.OrderItemList
                    .Select(group => "[" + string.Join(',', group) + "]")
                    .ToArray(),
                order_id = o.OrderId,
            }).ToArray(),
            delivery_type           = request.DeliveryType,
            shipment_provider_code  = request.ShipmentProviderCode,
            shipping_allocate_type  = request.ShippingAllocateType,
        });

        var lazRequest = new LazRequest("/order/fulfill/pack");
        lazRequest.AddApiParameter("packReq", payload);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<PackV2Response>();
    }

    public async Task<ReadyToShipV2Response> ReadyToShipV2Async(
        ReadyToShipV2Request request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (request.Packages is null || request.Packages.Count == 0)
        {
            throw new ArgumentException("At least one package is required.", nameof(request));
        }

        var payload = JsonSerializer.Serialize(new
        {
            packages = request.Packages.Select(p => new { package_id = p.PackageId }).ToArray(),
        });

        var lazRequest = new LazRequest("/order/package/rts");
        lazRequest.AddApiParameter("readyToShipReq", payload);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<ReadyToShipV2Response>();
    }

    public async Task<RecreatePackageResponse> RecreatePackageAsync(
        RecreatePackageRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (request.Packages is null || request.Packages.Count == 0)
        {
            throw new ArgumentException("At least one package is required.", nameof(request));
        }

        var payload = JsonSerializer.Serialize(new
        {
            packages = request.Packages.Select(p => new { package_id = p.PackageId }).ToArray(),
        });

        var lazRequest = new LazRequest("/order/package/repack");
        lazRequest.AddApiParameter("rePackReq", payload);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<RecreatePackageResponse>();
    }

    public Task<DbsPackageDeliveryResponse> ConfirmDbsDeliveryAsync(
        ConfirmDbsDeliveryRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
        => PostPackagesAsync<DbsPackageDeliveryResponse>("/order/package/sof/delivered", "dbsDeliveryReq", request?.Packages, accessToken, credentials, cancellationToken);

    public Task<DbsPackageDeliveryResponse> FailedDbsDeliveryAsync(
        FailedDbsDeliveryRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
        => PostPackagesAsync<DbsPackageDeliveryResponse>("/order/package/sof/failed_delivery", "dbsFailedDeliveryReq", request?.Packages, accessToken, credentials, cancellationToken);

    public async Task<PackageStatusUpdateForDbsResponse> PackageStatusUpdateForDbsAsync(
        PackageStatusUpdateForDbsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.TrackingNumber);
        ArgumentException.ThrowIfNullOrEmpty(request.Source);
        ArgumentException.ThrowIfNullOrEmpty(request.PackageId);
        ArgumentNullException.ThrowIfNull(request.TrackInfo);

        var trackInfoJson = JsonSerializer.Serialize(new
        {
            latestStatus = new
            {
                subStatusDesc = request.TrackInfo.LatestStatus.SubStatusDesc,
                subStatus     = request.TrackInfo.LatestStatus.SubStatus,
                status        = request.TrackInfo.LatestStatus.Status,
            },
            latestEvent = new
            {
                stage       = request.TrackInfo.LatestEvent.Stage,
                eventTime   = request.TrackInfo.LatestEvent.EventTime.ToString(System.Globalization.CultureInfo.InvariantCulture),
                description = request.TrackInfo.LatestEvent.Description,
                location    = request.TrackInfo.LatestEvent.Location,
            },
        });

        var lazRequest = new LazRequest("/order/package/sof/status/update");
        lazRequest.AddApiParameter("trackingNumber", request.TrackingNumber);
        lazRequest.AddApiParameter("source",         request.Source);
        if (!string.IsNullOrEmpty(request.CarrierCode))
        {
            lazRequest.AddApiParameter("carrierCode", request.CarrierCode);
        }
        lazRequest.AddApiParameter("tag",       request.PackageId);
        lazRequest.AddApiParameter("trackInfo", trackInfoJson);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<PackageStatusUpdateForDbsResponse>();
    }

    public async Task<DeliverDigitalResponse> DeliverDigitalAsync(
        DeliverDigitalRequest request,
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

        var payload = JsonSerializer.Serialize(new
        {
            orders = request.Orders.Select(o => new
            {
                order_item_list = o.OrderItemList
                    .Select(group => "[" + string.Join(',', group) + "]")
                    .ToArray(),
                order_id = o.OrderId,
            }).ToArray(),
        });

        var lazRequest = new LazRequest("/order/digital/delivered");
        lazRequest.AddApiParameter("digitalDeliveryReq", payload);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<DeliverDigitalResponse>();
    }

    private async Task<TResponse> PostPackagesAsync<TResponse>(
        string apiPath,
        string paramName,
        IReadOnlyList<AwbPackage>? packages,
        string accessToken,
        LazCredentials? credentials,
        CancellationToken cancellationToken)
        where TResponse : class
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (packages is null || packages.Count == 0)
        {
            throw new ArgumentException("At least one package is required.", nameof(packages));
        }

        var payload = JsonSerializer.Serialize(new
        {
            packages = packages.Select(p => new { package_id = p.PackageId }).ToArray(),
        });

        var lazRequest = new LazRequest(apiPath);
        lazRequest.AddApiParameter(paramName, payload);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<TResponse>();
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
