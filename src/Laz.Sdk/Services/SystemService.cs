using Laz.Sdk.Models.System;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class SystemService(LazClient client) : ISystemService
{
    private readonly LazClient _client = client;

    public async Task<LazAccessTokenWithOpenId> CreateAccessTokenWithOpenIdAsync(
        string code,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);

        var request = new LazRequest("/auth/token/createWithOpenId") { HttpMethod = Constants.METHOD_GET };
        request.AddApiParameter("code", code);

        var response = await _client.ExecuteCoreAsync(
            request,
            UrlConstants.API_AUTHORIZATION_URL,
            accessToken: null,
            timestamp: null,
            perCallCredentials: credentials,
            cancellationToken).ConfigureAwait(false);

        var token = response.ReadAs<LazAccessTokenWithOpenId>();
        if (token is null || string.IsNullOrEmpty(token.AccessToken))
        {
            var errCode = token?.Code ?? response.Code ?? "unknown";
            var errMsg = token?.Message ?? response.Message ?? "Lazada auth endpoint returned no access_token.";
            throw new LazException(errCode, errMsg);
        }
        return token;
    }

    public async Task<GetDataMopFormatResponse> GetDataMopFormatAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/data/mop/format/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetDataMopFormatResponse>();
    }
}
