using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Input;

namespace Kzrnm.Wpf.Input;
using static Key;
using static ModifierKeys;

public class ShortcutKeyConverterTest
{
    [Theory]
    [InlineData(ModifierKeys.None, Key.None, "")]
    [InlineData(ModifierKeys.None, K, "K")]
    [InlineData(Control, K, "Ctrl-K")]
    [InlineData(Control | Shift, K, "Ctrl+Shift-K")]
    public static void Serialize(ModifierKeys modifier, Key key, string text)
    {
        var shortcutKey = new ShortcutKey(modifier, key);
        JsonSerializer.Serialize(shortcutKey, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        }).Should().Be($"\"{text}\"");
    }

    [Fact]
    public static void SerializeArray()
    {
        JsonSerializer.Serialize(new ShortcutKey[]
        {
            new(ModifierKeys.None, K),
            new(Alt, K),
            new(Control, K),
            new(Shift, K),
            new(Windows, K),
        }, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        }).Should().Be(@"[""K"",""Alt-K"",""Ctrl-K"",""Shift-K"",""Windows-K""]");
    }

    [Fact]
    public static void SerializeAll()
    {
        var keyConverter = TypeDescriptor.GetConverter(typeof(Key));
        var modifierKeysConverter = TypeDescriptor.GetConverter(typeof(ModifierKeys));


        foreach (var key in Enum.GetValues<Key>())
            if (key is not Key.None)
            {
                Test(
                    new ShortcutKey(key),
                    $"{keyConverter.ConvertToInvariantString(key)}"
                );
                for (int modifierKeys = 1; modifierKeys < 16; modifierKeys++)
                    Test(
                        new ShortcutKey((ModifierKeys)modifierKeys, key),
                        $"{modifierKeysConverter.ConvertToInvariantString((ModifierKeys)modifierKeys)}-{keyConverter.ConvertToInvariantString(key)}"
                    );
            }

        static void Test(ShortcutKey shortcutKey, string text)
        {
            JsonSerializer.Serialize(shortcutKey, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            }).Should().Be($"\"{text}\"");
        }
    }


    [Theory]
    [InlineData(ModifierKeys.None, Key.None, "")]
    [InlineData(ModifierKeys.None, K, "K")]
    [InlineData(Control, K, "Ctrl-K")]
    [InlineData(Control | Shift, K, "Ctrl+Shift-K")]
    public static void Deserialize(ModifierKeys modifier, Key key, string text)
    {
        var shortcutKey = new ShortcutKey(modifier, key);
        JsonSerializer.Deserialize<ShortcutKey>($"\"{text}\"").Should().Be(shortcutKey);
    }

    [Fact]
    public static void DeserializeArray()
    {
        JsonSerializer.Deserialize<ShortcutKey[]>(@"[""K"",""Alt-K"",""Ctrl-K"",""Shift-K"",""Windows-K""]").Should()
            .Equal(new ShortcutKey[]
        {
            new(ModifierKeys.None, K),
            new(Alt, K),
            new(Control, K),
            new(Shift, K),
            new(Windows, K),
        });
    }
}