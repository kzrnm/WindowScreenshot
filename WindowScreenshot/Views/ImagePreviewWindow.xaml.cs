using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
}
