using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.RectCapturer.Models;
using Kzrnm.RectCapturer.Properties;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Windows;
using Kzrnm.Wpf.Input;
using System.Windows.Input;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(GlobalService globalOperations, ImageDropTarget.Factory imageDropTargetFactory)
    {
        GlobalOperations = globalOperations;
        ConfigMaster = globalOperations.ConfigMaster;
        ImageProvider = globalOperations.ImageProvider;
        DropHandler = imageDropTargetFactory.Build(true);
        ClipboardManager = globalOperations.ClipboardManager;
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
    public GlobalService GlobalOperations { get; }
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
    private bool CanAddImmageFromClipboard() => GlobalOperations.CanPasteImageFromClipboard;
    [RelayCommand(CanExecute = nameof(CanAddImmageFromClipboard))]
    private void PasteImageFromClipboard()
        => GlobalOperations.PasteImageFromClipboard();

    [RelayCommand]
    private void OnPreviewKeyDown(KeyEventArgs e)
    {
        var shortcuts = ConfigMaster.Shortcuts.Value;
        var input = new ShortcutKey(Keyboard.Modifiers, e.Key);
        if (new ShortcutKey(ModifierKeys.Control, Key.V) == input)
        {
            PasteImageFromClipboard();
        }
        if (shortcuts.CaptureScreenshot == input)
        {
            GlobalOperations.CaptureScreenshot();
        }
        if (shortcuts.Post == input)
        {
            GlobalOperations.PostContent();
        }
    }
}
