using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kzrnm.RectCapturer.Properties;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Windows;

namespace Kzrnm.RectCapturer.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(ImageProvider imageProvider, ImageDropTarget.Factory imageDropTargetFactory, IClipboardManager clipboardManager)
    {
        ImageProvider = imageProvider;
        DropHandler = imageDropTargetFactory.Build(true);
        ClipboardManager = clipboardManager;
        imageProvider.Images.SelectedChanged += (sender, e) =>
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
    private bool CanAddImmageFromClipboard() => ImageProvider.CanAddImage && ClipboardManager.ContainsImage();
    [RelayCommand(CanExecute = nameof(CanAddImmageFromClipboard))]
    private void PasteImageFromClipboard()
    {
        if (ClipboardManager.GetImage() is { } image)
            ImageProvider.AddImage(image);
    }
}
