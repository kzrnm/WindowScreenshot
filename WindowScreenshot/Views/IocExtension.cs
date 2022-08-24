using CommunityToolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Views;
public static class IocExtension
{
    public static T GetRequiredServiceIfIsNotInDesignMode<T>(this Ioc ioc, DependencyObject dependencyObject) where T : class
    {
        if (DesignerProperties.GetIsInDesignMode(dependencyObject)) return null!;
        return ioc.GetRequiredService<T>();
    }
    public static T? GetServiceIfIsNotInDesignMode<T>(this Ioc ioc, DependencyObject dependencyObject) where T : class
    {
        if (DesignerProperties.GetIsInDesignMode(dependencyObject)) return null;
        return ioc.GetService<T>();
    }
}
