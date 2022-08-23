using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Windows;
using System.Windows;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class ImagePreviewWindowViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
{
    private readonly IClipboardManager ClipboardManager;
    public ImageProvider ImageProvider { get; }
    public ImagePreviewWindowViewModel(ImageDropTarget.Factory imageDropTargetFactory, IClipboardManager clipboard, ImageProvider imageProvider)
        : this(WeakReferenceMessenger.Default, imageDropTargetFactory, clipboard, imageProvider)
    { }
    public ImagePreviewWindowViewModel(IMessenger messenger, ImageDropTarget.Factory imageDropTargetFactory, IClipboardManager clipboardManager, ImageProvider imageProvider)
        : base(messenger)
    {
        DropHandler = imageDropTargetFactory.Build(false);
        ImageProvider = imageProvider;
        ClipboardManager = clipboardManager;
        IsActive = true;
    }

    public ImageDropTarget DropHandler { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Visibility))]
    private CaptureImage? _SelectedImage;

    public Visibility Visibility => SelectedImage is null ? Visibility.Hidden : Visibility.Visible;

    [RelayCommand]
    private void CopyToClipboard(CaptureImage? img)
    {
        if (img is not null)
            ClipboardManager.SetImage(img.TransformedImage);
    }

    [RelayCommand]
    public void ClearImage()
    {
        ImageProvider.Images.Clear();
    }

    public void UpdateMenuCommandState()
    {
        pasteImageFromClipboardCommand?.NotifyCanExecuteChanged();
    }
    private bool CanAddImmageFromClipboard() => ImageProvider.CanAddImage && ClipboardManager.ContainsImage();
    [RelayCommand(CanExecute = nameof(CanAddImmageFromClipboard))]
    private void PasteImageFromClipboard()
    {
        if (ClipboardManager.GetImage() is { } image)
            ImageProvider.AddImage(image);
    }

    void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
    {
        SelectedImage = message.Value;
    }

}
