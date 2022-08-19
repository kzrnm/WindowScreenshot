using System;
using System.Windows;

namespace Kzrnm.Wpf.Font.Views;
/// <summary>
/// FontDialogWindow.xaml の相互作用ロジック
/// </summary>
public partial class FontDialogWindow : Window
{
    public FontDialogWindow(FontDialogParams fontParams)
    {
        InitializeComponent();
        SelectedFont = fontParams;
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        DataContext = new FontDialogViewModel(SelectedFont.FontName, SelectedFont.FontSize);
    }

    private FontDialogViewModel ViewModel => (FontDialogViewModel)DataContext;
    public FontDialogParams SelectedFont { get; private set; }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        var vm = ViewModel;
        SelectedFont = new(vm.FontSize, vm.FontName);
        DialogResult = true;
    }
}
