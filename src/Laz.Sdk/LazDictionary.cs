using System.Globalization;

namespace Laz.Sdk;

/// <summary>
/// Ordinal-keyed <see cref="Dictionary{TKey, TValue}"/> with typed <c>Add</c> overloads that
/// format every value with <see cref="CultureInfo.InvariantCulture"/>. Null or empty keys / values
/// are silently dropped (the platform rejects them).
/// </summary>
public sealed class LazDictionary : Dictionary<string, string>
{
    public LazDictionary() : base(StringComparer.Ordinal) { }

    public LazDictionary(IDictionary<string, string> source)
        : base(source, StringComparer.Ordinal) { }

    public new void Add(string key, string? value)
    {
        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
        {
            base[key] = value;
        }
    }

    public void Add(string key, object? value)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        var formatted = value switch
        {
            null => null,
            string s => s,
            DateTime dt => dt.ToUniversalTime().Ticks.ToString(CultureInfo.InvariantCulture),
            DateTimeOffset dto => dto.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture),
            bool b => b ? "true" : "false",
            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString(),
        };

        Add(key, formatted);
    }

    public void AddAll(IDictionary<string, string>? source)
    {
        if (source is null)
        {
            return;
        }
        foreach (var kvp in source)
        {
            Add(kvp.Key, kvp.Value);
        }
    }
}
