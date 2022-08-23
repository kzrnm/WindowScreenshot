using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.RectCapturer.Configs;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Models;
using System.Diagnostics;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class MainBodyViewModel : ObservableObject
{
    public MainBodyViewModel(ImageProvider imageProvider, ConfigMaster configMaster)
    {
        ImageProvider = imageProvider;
        ConfigMaster = configMaster;
    }
    public ImageProvider ImageProvider { get; }
    public ConfigMaster ConfigMaster { get; }

    [RelayCommand]
    private void OpenSelectCaptureWindowDialog()
    {
        var result = WeakReferenceMessenger.Default.Send(new CaptureTargetSelectionDialogMessage(ConfigMaster.CaptureTargetWindows.Value));
        if (result.Response is { } collection)
        {
            ConfigMaster.CaptureTargetWindows.Value = new CaptureWindowCollection(collection);
        }
    }
    [RelayCommand]
    private void OpenConfigDialog()
    {
        // TODO: OpenConfigDialog
        Debug.WriteLine("OpenConfigDialogCommand");
    }
    [RelayCommand]
    private void CaptureScreenshot()
    {
        // TODO: CaptureScreenshot
        Debug.WriteLine("CaptureScreenshotCommand");
    }
    [RelayCommand]
    private void PostContent()
    {
        // TODO: PostContent
        Debug.WriteLine("PostContentCommand");
    }

}
