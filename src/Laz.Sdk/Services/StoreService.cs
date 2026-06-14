using System.Globalization;
using Laz.Sdk.Models.Store;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class StoreService(ILazClient client) : IStoreService
{
    private readonly ILazClient _client = client;

    public async Task<GetStoreCustomPageResponse> GetStoreCustomPageAsync(
        int page,
        int size,
        string? keyword,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (page <= 0) throw new ArgumentException("Page must be greater than zero.", nameof(page));
        if (size <= 0) throw new ArgumentException("Size must be greater than zero.", nameof(size));

        var lazRequest = new LazRequest("/store/custom/page/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("page", page.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("size", size.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(keyword))
        {
            lazRequest.AddApiParameter("keyword", keyword);
        }

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetStoreCustomPageResponse>();
    }
}
