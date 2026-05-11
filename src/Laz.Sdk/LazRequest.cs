using Laz.Sdk.Util;

namespace Laz.Sdk;

/// <summary>
/// Lazada Open Platform request envelope.
/// </summary>
public sealed class LazRequest
{
    /// <summary>API name (e.g. <c>/system/oauth/refresh</c>).</summary>
    public string? ApiName { get; set; }

    /// <summary>HTTP method. Defaults to <c>POST</c>.</summary>
    public string HttpMethod { get; set; } = Constants.METHOD_POST;

    /// <summary>API parameters (request body fields or query-string fields).</summary>
    public LazDictionary ApiParams { get; } = new();

    /// <summary>File parameters for multipart uploads.</summary>
    public IDictionary<string, FileItem> FileParams { get; } = new Dictionary<string, FileItem>(StringComparer.Ordinal);

    /// <summary>Extra HTTP header parameters.</summary>
    public LazDictionary HeaderParams { get; } = new();

    public LazRequest() { }

    public LazRequest(string apiName) => ApiName = apiName;

    public void AddApiParameter(string key, string value) => ApiParams.Add(key, value);

    public void AddApiParameter(string key, object? value) => ApiParams.Add(key, value);

    public void AddFileParameter(string key, FileItem file) => FileParams[key] = file;

    public void AddHeaderParameter(string key, string value) => HeaderParams.Add(key, value);
}
