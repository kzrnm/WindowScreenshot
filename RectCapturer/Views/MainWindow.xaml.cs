using CommunityToolkit.Mvvm.DependencyInjection;
using Kzrnm.RectCapturer.Configs;
using System;
using System.Windows;

namespace Kzrnm.RectCapturer.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ConfigMaster = Ioc.Default.GetService<ConfigMaster>();
        if (ConfigMaster?.Config is { } confWrapper)
        {
            confWrapper.Value.WindowPosition.ApplyTo(this);
            Topmost = confWrapper.Value.Topmost;
            confWrapper.ConfigUpdated += (sender, e) =>
            {
                Topmost = e.Topmost;
            };
        }
    }

    private ConfigMaster? ConfigMaster { get; }

    protected override async void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        if (ConfigMaster is { } configMaster)
        {
            var newConfig = configMaster.Config.Value with
            {
                WindowPosition = WindowPosition.Load(this),
                ImagePreviewWindowPosition = WindowCapturer.ImagePreviewWindow switch
                {
                    { } ipw => WindowStartPosition.Load(ipw),
                    _ => null,
                },
            };

            configMaster.Config.Value = newConfig;
            await configMaster.SaveAsync().ConfigureAwait(false);
        }
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        if (WindowCapturer.ImagePreviewWindow is { } ipw)
            ConfigMaster?.Config.Value.ImagePreviewWindowPosition?.ApplyTo(ipw);
    }

    private void MovePreviewWindowHereClick(object sender, RoutedEventArgs e)
    {
        if (WindowCapturer.ImagePreviewWindow is { } ipw)
        {
            ipw.Top = Top;
            ipw.Left = Left;
        }
    }
}
