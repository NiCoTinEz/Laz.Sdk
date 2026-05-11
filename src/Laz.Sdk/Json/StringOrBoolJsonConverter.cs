using System.Text.Json;
using System.Text.Json.Serialization;

namespace Laz.Sdk.Json;

/// <summary>
/// Reads either a native JSON boolean (<c>true</c> / <c>false</c>) or a JSON string
/// (<c>"true"</c> / <c>"false"</c>) into a <see cref="bool"/>. Lazada inconsistently wires
/// boolean-looking fields as quoted strings.
/// </summary>
internal sealed class StringOrBoolJsonConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.True   => true,
            JsonTokenType.False  => false,
            JsonTokenType.String => bool.Parse(reader.GetString()!),
            _ => throw new JsonException($"Cannot convert {reader.TokenType} to bool."),
        };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        => writer.WriteBooleanValue(value);
}
