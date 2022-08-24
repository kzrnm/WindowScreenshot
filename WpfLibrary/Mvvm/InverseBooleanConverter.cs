using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Kzrnm.Wpf.Mvvm;

[ValueConversion(typeof(bool), typeof(bool))]
public class InverseBooleanConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b)
            return !b;
        throw new ArgumentException(nameof(value) + " must be bool");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b)
            return !b;
        throw new ArgumentException(nameof(value) + " must be bool");
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
