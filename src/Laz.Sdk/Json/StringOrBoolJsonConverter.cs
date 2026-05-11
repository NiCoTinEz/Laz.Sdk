using System.Text.Json;
using System.Text.Json.Serialization;

namespace Laz.Sdk.Json;

/// <summary>
/// Reads a <see cref="bool"/> from any of the wire forms Lazada uses:
/// native JSON <c>true</c>/<c>false</c>, the strings <c>"true"</c>/<c>"false"</c>
/// (any case), the strings <c>"1"</c>/<c>"0"</c>, and JSON numbers (non-zero → true).
/// </summary>
internal sealed class StringOrBoolJsonConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.True:  return true;
            case JsonTokenType.False: return false;
            case JsonTokenType.Number:
                return reader.TryGetInt64(out var l) ? l != 0L : reader.GetDouble() != 0.0;
            case JsonTokenType.String:
                var s = reader.GetString();
                if (string.IsNullOrEmpty(s)) return false;
                if (s.Equals("true",  StringComparison.OrdinalIgnoreCase)) return true;
                if (s.Equals("false", StringComparison.OrdinalIgnoreCase)) return false;
                if (s == "1") return true;
                if (s == "0") return false;
                throw new JsonException($"Cannot convert string '{s}' to bool.");
            default:
                throw new JsonException($"Cannot convert {reader.TokenType} to bool.");
        }
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        => writer.WriteBooleanValue(value);
}
