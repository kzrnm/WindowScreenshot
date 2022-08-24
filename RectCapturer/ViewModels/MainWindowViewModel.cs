using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.RectCapturer.Models;
using Kzrnm.RectCapturer.Properties;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Windows;
using Kzrnm.Wpf.Input;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(GlobalService globalService, ContentService contentService, ImageDropTarget.Factory imageDropTargetFactory)
    {
        GlobalService = globalService;
        ContentService = contentService;
        ConfigMaster = globalService.ConfigMaster;
        ImageProvider = globalService.ImageProvider;
        DropHandler = imageDropTargetFactory.Build(true);
        ClipboardManager = globalService.ClipboardManager;
        ImageProvider.Images.SelectedChanged += (sender, e) =>
        {
            switch ((e.OldItem, e.NewItem))
            {
                case (null, not null):
                case (not null, null):
                    OnPropertyChanged(nameof(PreviewWindowShown));
                    break;
            }
        };
    }
    public GlobalService GlobalService { get; }
    public ContentService ContentService { get; }
    public ConfigMaster ConfigMaster { get; }
    public ImageProvider ImageProvider { get; }
    public ImageDropTarget DropHandler { get; }
    public IClipboardManager ClipboardManager { get; }
    public string Title { get; } = Resources.MainWindowTitle;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewWindowShown))]
    private bool _HasPreviewWindow = true;

    [RelayCommand]
    private void ClearImage() => ImageProvider.Images.Clear();

    public void UpdateMenuCommandState()
    {
        pasteImageFromClipboardCommand?.NotifyCanExecuteChanged();
    }

    public bool PreviewWindowShown => HasPreviewWindow && ImageProvider.Images.SelectedItem != null;
    private bool CanAddImmageFromClipboard() => GlobalService.CanPasteImageFromClipboard;
    [RelayCommand(CanExecute = nameof(CanAddImmageFromClipboard))]
    private void PasteImageFromClipboard()
        => GlobalService.PasteImageFromClipboard();

    [RelayCommand]
    private async Task OnPreviewKeyDown(KeyEventArgs e)
    {
        var shortcuts = ConfigMaster.Shortcuts.Value;
        var input = new ShortcutKey(Keyboard.Modifiers, e.Key);
        if (GlobalService.CanPasteImageFromClipboard && new ShortcutKey(ModifierKeys.Control, Key.V) == input)
        {
            PasteImageFromClipboard();
            e.Handled = true;
        }
        if (shortcuts.CaptureScreenshot == input)
        {
            GlobalService.CaptureScreenshot();
            e.Handled = true;
        }
        if (ContentService.CanPost && shortcuts.Post == input)
        {
            await ContentService.PostContentAsync().ConfigureAwait(false);
            e.Handled = true;
        }
    }
}
