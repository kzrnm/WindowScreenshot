using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Kzrnm.Wpf.Mvvm;
public class EnumWithTextTest
{
    [Fact]
    public void DescriptionText()
    {
        new EnumValuesWithDescriptionText { Type = typeof(OnePiece) }.ProvideValue(null!)
            .Should()
            .BeOfType<EnumWithText[]>()
            .Equals(new EnumWithText[]
            {
                new(OnePiece.Luffy, "Mugiwara"),
                new(OnePiece.Shanks, "Akagami"),
                new(OnePiece.Kurohige, "Kurohige"),
                new(OnePiece.Buggy, "CrossGuild"),
            });
    }
    [Fact]
    public void ConverterText()
    {
        new EnumValuesWithConverterText { Type = typeof(OnePiece) }.ProvideValue(null!)
            .Should()
            .BeOfType<EnumWithText[]>()
            .Equals(new EnumWithText[]
            {
                new(OnePiece.Luffy, "Mugiwara-Luffy"),
                new(OnePiece.Shanks, "Akagami-Shanks"),
                new(OnePiece.Kurohige, "Kurohige-Kurohige"),
                new(OnePiece.Buggy, "CrossGuild-Buggy"),
            });
    }

    class OnePieceConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
            => destinationType == typeof(string);

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is OnePiece o)
            {
                var decription = typeof(OnePiece).GetField(o.ToString())!.GetCustomAttribute<DescriptionAttribute>(false)!.Description;
                return $"{decription}-{o}";
            }
            throw new NotSupportedException();
        }
    }

    [TypeConverter(typeof(OnePieceConverter))]
    public enum OnePiece
    {
        [Description("Mugiwara")]
        Luffy,
        [Description("Akagami")]
        Shanks,
        [Description("Kurohige")]
        Kurohige,
        [Description("CrossGuild")]
        Buggy,
    }
}
