using System.Globalization;
using System.Text;
using Laz.Sdk.Models.Orders;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class OrdersService(ILazClient client) : IOrdersService
{
    private readonly ILazClient _client = client;

    public async Task<GetOrderDocumentResponse> GetDocumentAsync(
        GetOrderDocumentRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (request.OrderItemIds is null || request.OrderItemIds.Count == 0)
        {
            throw new ArgumentException("OrderItemIds must contain at least one id.", nameof(request));
        }

        var lazRequest = new LazRequest("/order/document/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("doc_type", MapDocType(request.DocType));
        lazRequest.AddApiParameter("order_item_ids", FormatIds(request.OrderItemIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetOrderDocumentResponse>();
    }

    public async Task<GetOrdersResponse> GetOrdersAsync(
        GetOrdersRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (request.UpdateAfter is null && request.CreatedAfter is null)
        {
            throw new ArgumentException(
                "Either UpdateAfter or CreatedAfter is required.",
                nameof(request));
        }

        var lazRequest = new LazRequest("/orders/get") { HttpMethod = Constants.METHOD_GET };
        if (request.UpdateAfter   is { } ua) lazRequest.AddApiParameter("update_after",   FormatIso(ua));
        if (request.UpdateBefore  is { } ub) lazRequest.AddApiParameter("update_before",  FormatIso(ub));
        if (request.CreatedAfter  is { } ca) lazRequest.AddApiParameter("created_after",  FormatIso(ca));
        if (request.CreatedBefore is { } cb) lazRequest.AddApiParameter("created_before", FormatIso(cb));
        if (!string.IsNullOrEmpty(request.Status)) lazRequest.AddApiParameter("status", request.Status);
        if (request.Limit  is { } lim) lazRequest.AddApiParameter("limit",  lim.ToString(CultureInfo.InvariantCulture));
        if (request.Offset is { } off) lazRequest.AddApiParameter("offset", off.ToString(CultureInfo.InvariantCulture));
        if (request.SortBy        is { } sb) lazRequest.AddApiParameter("sort_by", MapSortBy(sb));
        if (request.SortDirection is { } sd) lazRequest.AddApiParameter("sort_direction", MapSortDirection(sd));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetOrdersResponse>();
    }

    public async Task<CancelOrderResponse> CancelAsync(
        CancelOrderRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/order/cancel");
        lazRequest.AddApiParameter("order_item_id", request.OrderItemId.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("reason_id",     request.ReasonId.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(request.ReasonDetail))
        {
            lazRequest.AddApiParameter("reason_detail", request.ReasonDetail);
        }

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<CancelOrderResponse>();
    }

    public async Task<ReadyToShipResponse> ReadyToShipAsync(
        ReadyToShipRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.ShipmentProvider);
        ArgumentException.ThrowIfNullOrEmpty(request.TrackingNumber);
        ArgumentException.ThrowIfNullOrEmpty(request.DeliveryType);
        if (request.OrderItemIds is null || request.OrderItemIds.Count == 0)
        {
            throw new ArgumentException("OrderItemIds must contain at least one id.", nameof(request));
        }

        var lazRequest = new LazRequest("/order/rts");
        lazRequest.AddApiParameter("delivery_type", request.DeliveryType);
        lazRequest.AddApiParameter("order_item_ids", FormatIds(request.OrderItemIds));
        lazRequest.AddApiParameter("shipment_provider", request.ShipmentProvider);
        lazRequest.AddApiParameter("tracking_number", request.TrackingNumber);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<ReadyToShipResponse>();
    }

    public async Task<PackOrderResponse> PackAsync(
        PackOrderRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(request.ShippingProvider);
        ArgumentException.ThrowIfNullOrEmpty(request.DeliveryType);
        if (request.OrderItemIds is null || request.OrderItemIds.Count == 0)
        {
            throw new ArgumentException("OrderItemIds must contain at least one id.", nameof(request));
        }

        var lazRequest = new LazRequest("/order/pack"); // default POST
        lazRequest.AddApiParameter("shipping_provider", request.ShippingProvider);
        lazRequest.AddApiParameter("delivery_type", request.DeliveryType);
        lazRequest.AddApiParameter("order_item_ids", FormatIds(request.OrderItemIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<PackOrderResponse>();
    }

    public async Task<GetMultipleOrderItemsResponse> GetMultipleOrderItemsAsync(
        GetMultipleOrderItemsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (request.OrderIds is null || request.OrderIds.Count == 0)
        {
            throw new ArgumentException("OrderIds must contain at least one id.", nameof(request));
        }
        if (request.OrderIds.Count > 50)
        {
            throw new ArgumentException("OrderIds cannot exceed 50 per call.", nameof(request));
        }

        var lazRequest = new LazRequest("/orders/items/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("order_ids", FormatIds(request.OrderIds));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetMultipleOrderItemsResponse>();
    }

    public async Task<GetOrderItemsResponse> GetOrderItemsAsync(
        GetOrderItemsRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/order/items/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("order_id", request.OrderId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetOrderItemsResponse>();
    }

    public async Task<GetOrderResponse> GetOrderAsync(
        GetOrderRequest request,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/order/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("order_id", request.OrderId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetOrderResponse>();
    }

    private static string FormatIso(DateTimeOffset value)
        => value.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture);

    private static string MapSortBy(OrderSortBy sortBy) => sortBy switch
    {
        OrderSortBy.CreatedAt => "created_at",
        OrderSortBy.UpdatedAt => "updated_at",
        _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null),
    };

    private static string MapSortDirection(OrderSortDirection direction) => direction switch
    {
        OrderSortDirection.Asc  => "ASC",
        OrderSortDirection.Desc => "DESC",
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
    };

    private static string MapDocType(OrderDocumentType docType) => docType switch
    {
        OrderDocumentType.Invoice         => "invoice",
        OrderDocumentType.ShippingLabel   => "shippingLabel",
        OrderDocumentType.CarrierManifest => "carrierManifest",
        _ => throw new ArgumentOutOfRangeException(nameof(docType), docType, "Unknown OrderDocumentType."),
    };

    private static string FormatIds(IReadOnlyCollection<long> ids)
    {
        var sb = new StringBuilder(2 + ids.Count * 8);
        sb.Append('[');
        var first = true;
        foreach (var id in ids)
        {
            if (!first)
            {
                sb.Append(',');
            }
            sb.Append(id.ToString(CultureInfo.InvariantCulture));
            first = false;
        }
        sb.Append(']');
        return sb.ToString();
    }
}
