using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Kzrnm.Wpf.Mvvm;


public class EnumValues : MarkupExtension
{
    public Type? Type { set; get; }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Type is null || !Type.IsEnum)
            throw new ArgumentException($"Type must be Enum");
        return Enum.GetValues(Type)
            .Cast<Enum>()
            .ToArray();
    }
}
public class EnumValuesWithDescriptionText : MarkupExtension
{
    public Type? Type { set; get; }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Type is null || !Type.IsEnum)
            throw new ArgumentException($"Type must be Enum");
        return Enum.GetValues(Type)
            .Cast<Enum>()
            .Select(e => new EnumWithText(e, Type.GetField(e.ToString())?.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? e.ToString()))
            .ToArray();
    }
}
public class EnumValuesWithConverterText : MarkupExtension
{
    public Type? Type { set; get; }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Type is null || !Type.IsEnum)
            throw new ArgumentException($"Type must be Enum");
        var converter = TypeDescriptor.GetConverter(Type);
        return Enum.GetValues(Type)
            .Cast<Enum>()
            .Select(e => new EnumWithText(e, (converter.CanConvertTo(typeof(string)) ? converter.ConvertToString(e) : null) ?? e.ToString()))
            .ToArray();
    }
}