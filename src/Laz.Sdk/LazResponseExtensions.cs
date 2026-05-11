using System.Text.Json;

namespace Laz.Sdk;

internal static class LazResponseExtensions
{
    /// <summary>
    /// Deserialize <see cref="LazResponse.Body"/> into <typeparamref name="T"/> or throw
    /// <see cref="LazException"/> if the platform reported an error or returned an empty body.
    /// </summary>
    public static T DeserializeOrThrow<T>(this LazResponse response, JsonSerializerOptions? options = null)
        where T : class
    {
        if (response.IsError())
        {
            throw new LazException(
                response.Code ?? "unknown",
                response.Message ?? "Lazada API returned an error.");
        }

        return response.ReadAs<T>(options)
            ?? throw new LazException("empty", "Lazada API returned an empty response body.");
    }
}
