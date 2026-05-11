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
