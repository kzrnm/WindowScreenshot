using System.Windows;

namespace Kzrnm.Wpf.Views;

/// <summary>
/// TextInputDialogWindow.xaml の相互作用ロジック
/// </summary>
public partial class TextInputDialogWindow : Window
{
    public TextInputDialogWindow()
    {
        InitializeComponent();
    }

    public string? ShowDialogWithResponse()
    {
        if (ShowDialog() != true)
            return null;

        return InputTextBox.Text;
    }


    public static readonly DependencyProperty MessageProperty
        = DependencyProperty.Register(
            nameof(Message),
            typeof(string),
            typeof(TextInputDialogWindow),
            new PropertyMetadata(null)
        );

    public string Message
    {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }
}
