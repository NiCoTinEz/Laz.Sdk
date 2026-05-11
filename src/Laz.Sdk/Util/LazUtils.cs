using System.Security.Cryptography;
using System.Text;

namespace Laz.Sdk.Util;

/// <summary>Lazada Open Platform helpers.</summary>
public static class LazUtils
{
    /// <summary>Sign an API request.</summary>
    public static string SignRequest(string apiName, IDictionary<string, string> parameters, string appSecret, string signMethod)
        => SignRequest(apiName, parameters, body: null, appSecret, signMethod);

    /// <summary>Sign an API request, optionally with a body fragment.</summary>
    public static string SignRequest(string apiName, IDictionary<string, string> parameters, string? body, string appSecret, string signMethod)
    {
        ArgumentNullException.ThrowIfNull(apiName);
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(appSecret);
        ArgumentNullException.ThrowIfNull(signMethod);

        var sorted = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);

        var query = new StringBuilder();
        query.Append(apiName);
        foreach (var kv in sorted)
        {
            if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
            {
                query.Append(kv.Key).Append(kv.Value);
            }
        }
        if (!string.IsNullOrEmpty(body))
        {
            query.Append(body);
        }

        if (!string.Equals(signMethod, Constants.SIGN_METHOD_SHA256, StringComparison.Ordinal))
        {
            throw new ArgumentException($"Unsupported sign method '{signMethod}'. Only '{Constants.SIGN_METHOD_SHA256}' is supported.", nameof(signMethod));
        }

        var key = Encoding.UTF8.GetBytes(appSecret);
        var data = Encoding.UTF8.GetBytes(query.ToString());
        var bytes = HMACSHA256.HashData(key, data);

        return Convert.ToHexString(bytes);
    }
}
