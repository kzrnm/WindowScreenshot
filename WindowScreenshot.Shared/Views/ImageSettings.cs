using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WindowScreenshot.ViewModels;

namespace WindowScreenshot.Views;
public class ImageSettings : Control
{
    static ImageSettings()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageSettings), new FrameworkPropertyMetadata(typeof(ImageSettings)));
    }

    public ImageSettings()
    {
        DataContext = Ioc.Default.GetRequiredServiceIfIsNotInDesignMode<ImageSettingsViewModel>(this);
    }
    public ImageSettingsViewModel ViewModel => (ImageSettingsViewModel)DataContext;
}
