using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.Wpf.Font;
using System.Windows;
using System.Windows.Controls;

namespace Kzrnm.WindowScreenshot.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if !DEBUG
        if(DebugArea.Parent is Panel panel) panel.Children.Remove(DebugArea);
#endif
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new FontDialogMessage(new Font()));
    }
}
