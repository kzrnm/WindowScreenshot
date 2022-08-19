using CommunityToolkit.Mvvm.ComponentModel;
using Kzrnm.Wpf.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Kzrnm.Wpf.Font;
public partial class FontDialogViewModel : ObservableValidator
{
    public FontDialogViewModel(string fontName, double fontSize)
    {
        FontName = string.IsNullOrEmpty(fontName) ? GetLocalFontName(SystemFonts.MessageFontFamily) : fontName;
        FontSize = fontSize;
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(FontDialogViewModel), nameof(ValidateFontName))]
    private string _FontName = "";

    [ObservableProperty]
    private double _FontSize = double.NaN;

    public static ValidationResult? ValidateFontName(string fontName, ValidationContext context)
    {
        if (string.IsNullOrEmpty(fontName))
            return ValidationResult.Success;
        var isValid = ((FontDialogViewModel)context.ObjectInstance).ValidateFontName(fontName);
        return isValid ? ValidationResult.Success : new(Resources.FontNotFoundErrorMessage);
    }

    private bool ValidateFontName(string fontName)
        => FontList.AsSpan().BinarySearch(fontName, FontListComparer) >= 0;
    private static readonly StringComparer FontListComparer = StringComparer.Ordinal;
    private static string[] LoadFontList()
    {
        var array = Fonts.SystemFontFamilies.Select(GetLocalFontName).ToArray();
        Array.Sort(array, FontListComparer);
        return array;
    }
    static string GetLocalFontName(FontFamily fontFamily)
    {
        var currentLang = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
        return
            fontFamily.FamilyNames.TryGetValue(currentLang, out var localName)
            ? localName
            : fontFamily.Source;
    }
    public string[] FontList { get; } = LoadFontList();

    public double[] FontSizeList { get; } = new double[] {
        10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 48, 64, 96,
    };
}
