using Laz.Sdk.Util;

namespace Laz.Sdk;

/// <summary>
/// Configuration for a single <see cref="ILazClient"/> registration.
/// <para>
/// <see cref="AppKey"/> and <see cref="AppSecret"/> are optional at registration time so that
/// multi-tenant apps can register only <see cref="ServerUrl"/> and supply credentials per
/// request via <see cref="ILazClient.WithCredentials"/>. Calls without either source of
/// credentials throw <see cref="InvalidOperationException"/> from the client.
/// </para>
/// </summary>
public sealed class LazClientOptions
{
    /// <summary>Lazada Open Platform app key. Leave empty to require <see cref="ILazClient.WithCredentials"/> per call.</summary>
    public string AppKey { get; set; } = "";

    /// <summary>Lazada Open Platform app secret. Leave empty to require <see cref="ILazClient.WithCredentials"/> per call.</summary>
    public string AppSecret { get; set; } = "";

    /// <summary>Regional gateway URL. Defaults to <see cref="UrlConstants.API_GATEWAY_URL_SG"/>.</summary>
    public string ServerUrl { get; set; } = UrlConstants.API_GATEWAY_URL_SG;

    /// <summary>Signing method. Only <c>sha256</c> is supported by the platform today.</summary>
    public string SignMethod { get; set; } = Constants.SIGN_METHOD_SHA256;

    /// <summary>HTTP timeout applied to the underlying <see cref="HttpClient"/>.</summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
}
