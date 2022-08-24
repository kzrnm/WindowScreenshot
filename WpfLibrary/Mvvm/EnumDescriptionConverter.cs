using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;

namespace Kzrnm.Wpf.Mvvm;

[ValueConversion(typeof(Enum), typeof(string))]
public class EnumDescriptionConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var attribute = value.GetType().GetField(value.ToString()!)?.GetCustomAttribute<DescriptionAttribute>(false);
        if (attribute != null)
            return attribute.Description;
        return value.ToString()!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
