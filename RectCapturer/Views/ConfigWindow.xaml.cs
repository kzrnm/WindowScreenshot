using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.RectCapturer.Configs;
using Kzrnm.RectCapturer.ViewModels;
using Kzrnm.WindowScreenshot.Views;
using System;
using System.ComponentModel;
using System.Windows;

namespace Kzrnm.RectCapturer.Views;

/// <summary>
/// ConfigWindow.xaml の相互作用ロジック
/// </summary>
public partial class ConfigWindow : Window
{
    public ConfigWindow(Config config, Shortcuts shortcuts)
    {
        InitializeComponent();
        DataContext = new ConfigWindowViewModel(config, shortcuts);
    }

    public (Config Config, Shortcuts Shortcuts)? ShowDialogWithResponse()
    {
        if (ShowDialog() != true)
            return null;

        return (DataContext as ConfigWindowViewModel)?.ToResult();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (DialogResult == null && (DataContext as ConfigWindowViewModel)?.IsUpdated is true)
        {
            var result = MessageBox.Show(this, WindowScreenshot.Properties.Resources.SaveChangesMessage, AppDomain.CurrentDomain.FriendlyName, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    DialogResult = true;
                    break;
                case MessageBoxResult.No:
                    DialogResult = false;
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }
    }
}
