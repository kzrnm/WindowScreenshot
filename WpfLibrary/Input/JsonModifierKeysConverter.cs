using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace Kzrnm.Wpf.Input;

public class JsonModifierKeysConverter : JsonConverter<ModifierKeys>
{
    private static TypeConverter ModifiersConverter => TypeDescriptor.GetConverter(typeof(ModifierKeys));

    public override ModifierKeys Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Number)
        {
            return (ModifierKeys)reader.GetInt64();
        }
        else if (reader.TokenType is JsonTokenType.String)
        {
            var value = reader.GetString();
            if (value == null) return ModifierKeys.None;
            return (ModifierKeys)(ModifiersConverter.ConvertFrom(value) ?? ModifierKeys.None);
        }
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, ModifierKeys value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(ModifiersConverter.ConvertToString(value));
    }
}