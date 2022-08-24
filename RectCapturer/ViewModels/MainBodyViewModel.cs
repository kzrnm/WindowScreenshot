using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.RectCapturer.Configs;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.WindowScreenshot.Models;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class MainBodyViewModel : ObservableObject
{
    public MainBodyViewModel(ImageProvider imageProvider, IObserveWindowProcess observeWindowProcess, ConfigMaster configMaster)
    {
        ImageProvider = imageProvider;
        ConfigMaster = configMaster;
        ObserveWindowProcess = observeWindowProcess;
    }
    public ImageProvider ImageProvider { get; }
    public ConfigMaster ConfigMaster { get; }
    public IObserveWindowProcess ObserveWindowProcess { get; }
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
        var result = WeakReferenceMessenger.Default.Send(new ConfigDialogMessage(ConfigMaster.Config.Value, ConfigMaster.Shortcuts.Value));
        if (result.Response is var (config, shortcuts) && config is not null && shortcuts is not null)
        {
            ConfigMaster.Config.Value = config;
            ConfigMaster.Shortcuts.Value = shortcuts;
        }
    }
    [RelayCommand]
    private void CaptureScreenshot()
    {
        var image = ConfigMaster.CaptureTargetWindows.Value
            .Select(ct => ct.CaptureFrom(ObserveWindowProcess.CurrentWindows))
            .OfType<BitmapSource>()
            .FirstOrDefault();

        if (image is not null)
        {
            ImageProvider.AddImage(image);
        }
    }
    [RelayCommand]
    private void PostContent()
    {
        // TODO: PostContent
        Debug.WriteLine("PostContentCommand");
    }

}
