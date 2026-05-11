using System.ComponentModel.DataAnnotations;
using Laz.Sdk.Util;

namespace Laz.Sdk;

/// <summary>
/// Configuration for a single <see cref="ILazClient"/> registration.
/// </summary>
public sealed class LazClientOptions
{
    /// <summary>Lazada Open Platform app key.</summary>
    [Required]
    public required string AppKey { get; set; }

    /// <summary>Lazada Open Platform app secret. Used for HMAC-SHA256 request signing.</summary>
    [Required]
    public required string AppSecret { get; set; }

    /// <summary>Regional gateway URL. Defaults to <see cref="UrlConstants.API_GATEWAY_URL_SG"/>.</summary>
    public string ServerUrl { get; set; } = UrlConstants.API_GATEWAY_URL_SG;

    /// <summary>Signing method. Only <c>sha256</c> is supported by the platform today.</summary>
    public string SignMethod { get; set; } = Constants.SIGN_METHOD_SHA256;

    /// <summary>HTTP timeout applied to the underlying <see cref="HttpClient"/>.</summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
}
