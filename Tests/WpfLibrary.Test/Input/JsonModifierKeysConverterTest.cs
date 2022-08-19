using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Input;

namespace Kzrnm.Wpf.Input;
using static ModifierKeys;

public class JsonModifierKeysConverterTest
{
    [Theory]
    [InlineData(None, "")]
    [InlineData(Alt, "Alt")]
    [InlineData(Control, "Ctrl")]
    [InlineData(Shift, "Shift")]
    [InlineData(Windows, "Windows")]
    [InlineData(Alt | Shift, "Alt+Shift")]
    [InlineData(Control | Shift, "Ctrl+Shift")]
    [InlineData(Control | Alt | Shift, "Ctrl+Alt+Shift")]
    public static void Serialize(ModifierKeys modifier, string text)
    {
        JsonSerializer.Serialize(modifier, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = {
                new JsonModifierKeysConverter(),
            },
        }).Should().Be($"\"{text}\"");
    }

    [Fact]
    public static void SerializeArray()
    {
        JsonSerializer.Serialize(new ModifierKeys[]
        {
            None,
            Alt,
            Control,
            Shift,
            Windows,
        }, new JsonSerializerOptions
        {
            Converters = {
                new JsonModifierKeysConverter(),
            }
        }).Should().Be(@"["""",""Alt"",""Ctrl"",""Shift"",""Windows""]");
    }

    [Theory]
    [InlineData(None, "")]
    [InlineData(Alt, "Alt")]
    [InlineData(Control, "Ctrl")]
    [InlineData(Shift, "Shift")]
    [InlineData(Windows, "Windows")]
    [InlineData(Alt | Shift, "Alt+Shift")]
    [InlineData(Control | Shift, "Ctrl+Shift")]
    [InlineData(Control | Alt | Shift, "Ctrl+Alt+Shift")]
    public static void Deserialize(ModifierKeys modifier, string text)
    {
        JsonSerializer.Deserialize<ModifierKeys>($"\"{text}\"", new JsonSerializerOptions
        {
            Converters = {
                new JsonModifierKeysConverter(),
            },
        }).Should().Be(modifier);
    }

    [Theory]
    [InlineData(None)]
    [InlineData(Alt)]
    [InlineData(Control)]
    [InlineData(Shift)]
    [InlineData(Windows)]
    [InlineData(Alt | Shift)]
    [InlineData(Control | Shift)]
    [InlineData(Control | Alt | Shift)]
    public static void DeserializeNumber(ModifierKeys modifier)
    {
        JsonSerializer.Deserialize<ModifierKeys>($"{(long)modifier}", new JsonSerializerOptions
        {
            Converters = {
                new JsonModifierKeysConverter(),
            },
        }).Should().Be(modifier);
    }

    [Fact]
    public static void DeserializeArray()
    {
        JsonSerializer.Deserialize<ModifierKeys[]>(@"["""",""Alt"",""Ctrl"",""Shift"",""Windows""]", new JsonSerializerOptions
        {
            Converters = {
                new JsonModifierKeysConverter(),
            }
        }).Should().Equal(new ModifierKeys[]
        {
            None,
            Alt,
            Control,
            Shift,
            Windows,
        });
    }
}