using Kzrnm.WindowScreenshot.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Kzrnm.WindowScreenshot.Views;
/// <summary>
/// ImagePreviewWindow.xaml の相互作用ロジック
/// </summary>
public partial class ImagePreviewWindow : Window
{
    public ImagePreviewWindow()
    {
        InitializeComponent();
    }
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        DragMove();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        if (Owner != null)
            e.Cancel = true;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        if (DataContext is ImagePreviewWindowViewModel vm)
        {
            vm.IsActive = false;
            DataContext = null;
        }
    }
}
