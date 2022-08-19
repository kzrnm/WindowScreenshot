using System;
using System.Windows;

namespace Kzrnm.Wpf.Font.Views;
/// <summary>
/// FontDialogWindow.xaml の相互作用ロジック
/// </summary>
public partial class FontDialogWindow : Window
{
    public FontDialogWindow(Font fontParams)
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
    public Font SelectedFont { get; private set; }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    public Font? ShowDialogWithResponse()
    {
        if (ShowDialog() != true)
            return null;

        var vm = ViewModel;
        return new(vm.FontName, vm.FontSize);
    }
}
