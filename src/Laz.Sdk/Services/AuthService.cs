using Laz.Sdk.Models;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class AuthService(LazClient client) : IAuthService
{
    private readonly LazClient _client = client;

    public async Task<LazAccessToken> CreateAccessTokenAsync(
        string code,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);

        var request = new LazRequest("/auth/token/create") { HttpMethod = Constants.METHOD_GET };
        request.AddApiParameter("code", code);

        var response = await _client.ExecuteCoreAsync(
            request,
            UrlConstants.API_AUTHORIZATION_URL,
            accessToken: null,
            timestamp: null,
            perCallCredentials: credentials,
            cancellationToken).ConfigureAwait(false);

        return ParseAuthResponse(response);
    }

    public async Task<LazAccessToken> RefreshAccessTokenAsync(
        string refreshToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshToken);

        var request = new LazRequest("/auth/token/refresh") { HttpMethod = Constants.METHOD_GET };
        request.AddApiParameter("refresh_token", refreshToken);

        var response = await _client.ExecuteCoreAsync(
            request,
            UrlConstants.API_AUTHORIZATION_URL,
            accessToken: null,
            timestamp: null,
            perCallCredentials: credentials,
            cancellationToken).ConfigureAwait(false);

        return ParseAuthResponse(response);
    }

    private static LazAccessToken ParseAuthResponse(LazResponse response)
    {
        var token = response.ReadAs<LazAccessToken>();
        if (token is null || string.IsNullOrEmpty(token.AccessToken))
        {
            var code    = token?.Code ?? response.Code ?? "unknown";
            var message = token?.Message ?? response.Message ?? "Lazada auth endpoint returned no access_token.";
            throw new LazException(code, message);
        }
        return token;
    }
}
