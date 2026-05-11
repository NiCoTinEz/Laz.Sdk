using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Laz.Sdk.Models;
using Laz.Sdk.Services;
using Laz.Sdk.Util;

namespace Laz.Sdk;

internal sealed class LazClient : ILazClient
{
    private const string PartnerId = "laz-sdk-net-20260511";

    private readonly HttpClient http;
    private readonly LazClientOptions options;

    public LazClient(HttpClient http, LazClientOptions options)
    {
        this.http = http;
        this.options = options;
        Orders = new OrdersService(this);
    }

    public IOrdersService Orders { get; }

    public Task<LazResponse> ExecuteAsync(
        LazRequest request,
        string? accessToken = null,
        DateTime? timestamp = null,
        CancellationToken cancellationToken = default)
        => ExecuteCoreAsync(request, options.ServerUrl, accessToken, timestamp, cancellationToken);

    public async Task<LazAccessToken> CreateAccessTokenAsync(string code, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);

        var request = new LazRequest("/auth/token/create") { HttpMethod = Constants.METHOD_GET };
        request.AddApiParameter("code", code);

        var response = await ExecuteCoreAsync(
            request,
            UrlConstants.API_AUTHORIZATION_URL,
            accessToken: null,
            timestamp: null,
            cancellationToken).ConfigureAwait(false);

        return ParseAuthResponse(response);
    }

    public async Task<LazAccessToken> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshToken);

        var request = new LazRequest("/auth/token/refresh") { HttpMethod = Constants.METHOD_GET };
        request.AddApiParameter("refresh_token", refreshToken);

        var response = await ExecuteCoreAsync(
            request,
            UrlConstants.API_AUTHORIZATION_URL,
            accessToken: null,
            timestamp: null,
            cancellationToken).ConfigureAwait(false);

        return ParseAuthResponse(response);
    }

    private async Task<LazResponse> ExecuteCoreAsync(
        LazRequest request,
        string serverUrl,
        string? accessToken,
        DateTime? timestamp,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrEmpty(request.ApiName))
        {
            throw new ArgumentException($"{nameof(LazRequest)}.{nameof(LazRequest.ApiName)} is required.", nameof(request));
        }

        var sysParams = new LazDictionary(request.ApiParams);
        sysParams.Add(Constants.APP_KEY, options.AppKey);
        sysParams.Add(Constants.SIGN_METHOD, options.SignMethod);
        sysParams.Add(Constants.TIMESTAMP, ResolveTimestampMs(timestamp));
        sysParams.Add(Constants.PARTNER_ID, PartnerId);
        if (!string.IsNullOrEmpty(accessToken))
        {
            sysParams.Add(Constants.ACCESS_TOKEN, accessToken);
        }

        var sign = LazUtils.SignRequest(request.ApiName, sysParams, options.AppSecret, options.SignMethod);
        sysParams.Add(Constants.SIGN, sign);

        var url = BuildServerUrl(serverUrl, request.ApiName);
        using var httpRequest = BuildHttpRequest(request, url, sysParams);
        ApplyHeaders(httpRequest, request);

        using var response = await http.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        return ParseResponse(body);
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

    private static long ResolveTimestampMs(DateTime? timestamp)
    {
        if (timestamp is null)
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        var dt = timestamp.Value;
        var utc = dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc),
        };
        return new DateTimeOffset(utc, TimeSpan.Zero).ToUnixTimeMilliseconds();
    }

    private static string BuildServerUrl(string serverUrl, string apiName)
    {
        var endsSlash = serverUrl.EndsWith('/');
        var startsSlash = apiName.StartsWith('/');
        return (endsSlash, startsSlash) switch
        {
            (true, true) => serverUrl + apiName[1..],
            (false, false) => serverUrl + "/" + apiName,
            _ => serverUrl + apiName,
        };
    }

    private static HttpRequestMessage BuildHttpRequest(LazRequest request, string url, LazDictionary sysParams)
    {
        if (request.FileParams.Count > 0)
        {
            var msg = new HttpRequestMessage(HttpMethod.Post, url);
            var multipart = new MultipartFormDataContent();
            foreach (var kv in sysParams)
            {
                multipart.Add(new StringContent(kv.Value, Encoding.UTF8), kv.Key);
            }
            foreach (var fp in request.FileParams)
            {
                if (!fp.Value.IsValid())
                {
                    throw new ArgumentException($"FileItem '{fp.Key}' is invalid.", nameof(request));
                }
                var content = new StreamContent(fp.Value.OpenRead());
                content.Headers.ContentType = new MediaTypeHeaderValue(fp.Value.GetMimeType());
                multipart.Add(content, fp.Key, fp.Value.GetFileName());
            }
            msg.Content = multipart;
            return msg;
        }

        if (string.Equals(request.HttpMethod, Constants.METHOD_GET, StringComparison.OrdinalIgnoreCase))
        {
            return new HttpRequestMessage(HttpMethod.Get, AppendQuery(url, sysParams));
        }

        return new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(sysParams),
        };
    }

    private static string AppendQuery(string url, IDictionary<string, string> parameters)
    {
        if (parameters.Count == 0)
        {
            return url;
        }

        var sb = new StringBuilder(url);
        var first = !url.Contains('?', StringComparison.Ordinal);
        sb.Append(first ? '?' : '&');

        var wroteAny = false;
        foreach (var kv in parameters)
        {
            if (string.IsNullOrEmpty(kv.Key) || string.IsNullOrEmpty(kv.Value))
            {
                continue;
            }
            if (wroteAny)
            {
                sb.Append('&');
            }
            sb.Append(Uri.EscapeDataString(kv.Key))
              .Append('=')
              .Append(Uri.EscapeDataString(kv.Value));
            wroteAny = true;
        }
        return sb.ToString();
    }

    private static void ApplyHeaders(HttpRequestMessage msg, LazRequest request)
    {
        if (request.HeaderParams.Count == 0)
        {
            return;
        }
        foreach (var kv in request.HeaderParams)
        {
            if (!msg.Headers.TryAddWithoutValidation(kv.Key, kv.Value))
            {
                msg.Content?.Headers.TryAddWithoutValidation(kv.Key, kv.Value);
            }
        }
    }

    private static LazResponse ParseResponse(string body)
    {
        if (string.IsNullOrEmpty(body))
        {
            return new LazResponse { Body = body };
        }

        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;
        return new LazResponse
        {
            Type      = TryGetString(root, Constants.RSP_TYPE),
            Code      = TryGetString(root, Constants.RSP_CODE),
            Message   = TryGetString(root, Constants.RSP_MSG),
            RequestId = TryGetString(root, Constants.RSP_REQUEST_ID),
            Body      = body,
        };
    }

    private static string? TryGetString(JsonElement root, string name)
        => root.ValueKind == JsonValueKind.Object
           && root.TryGetProperty(name, out var element)
           && element.ValueKind == JsonValueKind.String
            ? element.GetString()
            : null;
}
