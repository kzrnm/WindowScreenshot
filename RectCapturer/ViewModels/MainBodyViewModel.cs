using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.RectCapturer.Configs;
using Kzrnm.RectCapturer.Models;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Models;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class MainBodyViewModel : ObservableObject
{
    public MainBodyViewModel(GlobalService globalOperations, ImageDropTarget.Factory imageDropTargetFactory)
    {
        GlobalOperations = globalOperations;
        ImageProvider = globalOperations.ImageProvider;
        ConfigMaster = globalOperations.ConfigMaster;
        DropHandler = imageDropTargetFactory.Build(true);
    }
    public ImageDropTarget DropHandler { get; }
    public GlobalService GlobalOperations { get; }
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
        var result = WeakReferenceMessenger.Default.Send(new ConfigDialogMessage(ConfigMaster.Config.Value, ConfigMaster.Shortcuts.Value));
        if (result.Response is var (config, shortcuts) && config is not null && shortcuts is not null)
        {
            ConfigMaster.Config.Value = config;
            ConfigMaster.Shortcuts.Value = shortcuts;
        }
    }
    [RelayCommand]
    private void CaptureScreenshot()
        => GlobalOperations.CaptureScreenshot();

    [RelayCommand]
    private void PostContent()
        => GlobalOperations.PostContent();
}
