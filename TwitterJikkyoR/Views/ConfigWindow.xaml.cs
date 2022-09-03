using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.TwitterJikkyo.Configs;
using Kzrnm.TwitterJikkyo.ViewModels;
using Kzrnm.WindowScreenshot.Views;
using System;
using System.ComponentModel;
using System.Windows;

namespace Kzrnm.TwitterJikkyo.Views;

/// <summary>
/// ConfigWindow.xaml の相互作用ロジック
/// </summary>
public partial class ConfigWindow : Window
{
    public ConfigWindow(Config config, Shortcuts shortcuts)
    {
        InitializeComponent();
        var factory = Ioc.Default.GetRequiredServiceIfIsNotInDesignMode<ConfigWindowViewModel.Factory>(this);
        DataContext = factory.Build(config, shortcuts);
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
