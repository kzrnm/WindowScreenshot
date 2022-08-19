using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using Kzrnm.WindowScreenshot.ViewModels;

namespace Kzrnm.WindowScreenshot.Views;

public class ImageListView : Control
{
    static ImageListView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageListView), new FrameworkPropertyMetadata(typeof(ImageListView)));
    }
    public ImageListView()
    {
        DataContext = Ioc.Default.GetRequiredServiceIfIsNotInDesignMode<ImageListViewModel>(this);
        MinHeight = 40;
    }

    public ImageListViewModel ViewModel => (ImageListViewModel)DataContext;
}
