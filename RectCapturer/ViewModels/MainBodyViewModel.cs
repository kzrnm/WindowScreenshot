using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.RectCapturer.Configs;
using Kzrnm.RectCapturer.Models;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Models;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class MainBodyViewModel : ObservableObject
{
    public MainBodyViewModel(GlobalService globalService, ContentService contentService, ImageDropTarget.Factory imageDropTargetFactory)
    {
        GlobalService = globalService;
        ContentService = contentService;
        ImageProvider = globalService.ImageProvider;
        ConfigMaster = globalService.ConfigMaster;
        DropHandler = imageDropTargetFactory.Build(true);

        SaveDstDirectories = ConfigMaster.Config.Value.SaveDstDirectories;
        SaveFileNames = ConfigMaster.Config.Value.SaveFileNames;
        contentService.CanPostChanged += (_, _) => postContentCommand?.NotifyCanExecuteChanged();
    }
    public ContentService ContentService { get; }
    public ImageDropTarget DropHandler { get; }
    public GlobalService GlobalService { get; }
    public ImageProvider ImageProvider { get; }
    public ConfigMaster ConfigMaster { get; }

    [ObservableProperty]
    public ImmutableArray<string> _SaveDstDirectories;
    [ObservableProperty]
    public ImmutableArray<string> _SaveFileNames;

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
        => GlobalService.CaptureScreenshot();

    private bool CanPostContent() => ContentService.CanPost;
    [RelayCommand(CanExecute = nameof(CanPostContent))]
    private async Task PostContent()
        => await ContentService.PostContentAsync().ConfigureAwait(false);
}
