using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace Kzrnm.Wpf.Input;

[JsonConverter(typeof(JsonConverter))]
public record struct ShortcutKey(ModifierKeys Modifiers, Key Key)
{
    public ShortcutKey(Key key) : this(ModifierKeys.None, key) { }

    private static readonly TypeConverter keyConverter = TypeDescriptor.GetConverter(typeof(Key));
    private static readonly TypeConverter modifierKeysConverter = TypeDescriptor.GetConverter(typeof(ModifierKeys));
    public class JsonConverter : JsonConverter<ShortcutKey>
    {
        public override ShortcutKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var text = reader.GetString();
            if (text is null or "") return default;

            var splited = text.Split('-');
            if (splited.Length == 1)
                return new ShortcutKey((Key)keyConverter.ConvertFromInvariantString(splited[0])!);

            return new ShortcutKey(
                (ModifierKeys)modifierKeysConverter.ConvertFromInvariantString(splited[0])!,
                (Key)keyConverter.ConvertFromInvariantString(splited[1])!
                );
        }

        public override void Write(Utf8JsonWriter writer, ShortcutKey value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.CreateText('-', stackalloc char[80]));
        }
    }

    public override string ToString() => CreateTextString('+', stackalloc char[80]);

    private unsafe string CreateTextString(char sep, Span<char> buffer = default)
    {
        var span = CreateText(sep, buffer);
#if NETFRAMEWORK
        fixed (char* p = span)
            return new(p, 0, span.Length);
#else
        return new(span);
#endif
    }

    private ReadOnlySpan<char> CreateText(char sep, Span<char> buffer = default)
    {
        var modifiersText = modifierKeysConverter.ConvertToInvariantString(Modifiers) ?? "";
        var keyText = keyConverter.ConvertToInvariantString(Key) ?? "";
        if (modifiersText.Length + keyText.Length + 1 == buffer.Length)
            buffer = new char[modifiersText.Length + keyText.Length + 1];

        int bufIndex = 0;
        if (modifiersText.Length > 0)
        {
            modifiersText.AsSpan().CopyTo(buffer);
            bufIndex += modifiersText.Length;
            buffer[bufIndex++] = sep;
        }

        if (keyText.Length > 0)
        {
            keyText.AsSpan().CopyTo(buffer[bufIndex..]);
            bufIndex += keyText.Length;
        }
        return buffer[..bufIndex];
    }
}
