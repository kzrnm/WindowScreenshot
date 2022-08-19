using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows;

namespace Kzrnm.WindowScreenshot.DependencyInjection;

public static class IocBehavior
{
    public static Ioc Ioc { set; get; } = Ioc.Default;

    public static Type GetAutoViewModel(DependencyObject obj) => (Type)obj.GetValue(AutoViewModelProperty);
    public static void SetAutoViewModel(DependencyObject obj, Type value) => obj.SetValue(AutoViewModelProperty, value);
    public static readonly DependencyProperty AutoViewModelProperty =
        DependencyProperty.RegisterAttached(
            "AutoViewModel",
            typeof(Type),
            typeof(IocBehavior),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.NotDataBindable,
                AutoViewModelChanged));

    private static void AutoViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d))
            return;
        if (d is FrameworkElement elm && e.NewValue is Type type)
            elm.DataContext = Ioc.GetService(type);
    }
}
