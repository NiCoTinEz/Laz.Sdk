using System.Globalization;
using Laz.Sdk.Models.CrossBorder;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class CrossBorderService(ILazClient client) : ICrossBorderService
{
    private readonly ILazClient _client = client;

    public async Task<CreateGlobalProductResponse> CreateGlobalProductAsync(
        string payload,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/global/create");
        lazRequest.AddApiParameter("payload", payload);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<CreateGlobalProductResponse>();
    }

    public async Task<GetGlobalProductExtensionResponse> GetGlobalProductExtensionAsync(
        IReadOnlyList<long>? globalItemIds,
        IReadOnlyList<long>? itemIds,
        string country,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(country);

        var lazRequest = new LazRequest("/product/global/extension") { HttpMethod = Constants.METHOD_GET };
        if (globalItemIds is { Count: > 0 })
        {
            lazRequest.AddApiParameter("global_item_ids", string.Join(",", globalItemIds.Select(x => x.ToString(CultureInfo.InvariantCulture))));
        }
        if (itemIds is { Count: > 0 })
        {
            lazRequest.AddApiParameter("item_ids", string.Join(",", itemIds.Select(x => x.ToString(CultureInfo.InvariantCulture))));
        }
        lazRequest.AddApiParameter("country", country);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetGlobalProductExtensionResponse>();
    }

    public async Task<UpdateGlobalSkuResponse> UpdateGlobalSkuAsync(
        string payload,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/global/sku/update");
        lazRequest.AddApiParameter("payload", payload);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<UpdateGlobalSkuResponse>();
    }

    public async Task<GetGlobalSellerStatusResponse> GetGlobalSellerStatusAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/product/global/seller/status") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetGlobalSellerStatusResponse>();
    }
}
