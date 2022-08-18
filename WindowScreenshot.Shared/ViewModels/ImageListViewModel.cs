using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WindowScreenshot.Image;
using WindowScreenshot.Windows;

namespace WindowScreenshot.ViewModels;

public partial class ImageListViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
{
    private readonly IClipboardManager clipboard;
    public ImageProvider ImageProvider { get; }
    public ImageListViewModel(ICaptureImageService captureImageService, IClipboardManager clipboardManager, ImageProvider imageProvider)
        : this(WeakReferenceMessenger.Default, captureImageService, clipboardManager, imageProvider)
    { }
    public ImageListViewModel(IMessenger messenger, ICaptureImageService captureImageService, IClipboardManager clipboardManager, ImageProvider imageProvider)
        : base(messenger)
    {
        ImageProvider = imageProvider;
        clipboard = clipboardManager;
        DropHandler = new(captureImageService, imageProvider);
        DragHandler = new();
        IsActive = true;
    }

    public ImageDropTarget DropHandler { get; }
    public ImageDragSource DragHandler { get; }

    private bool ImageProviderHasImage() => ImageProvider.SelectedImageIndex >= 0;
    [RelayCommand(CanExecute = nameof(ImageProviderHasImage))]
    private void RemoveSelectedImage()
    {
        ImageProvider.Images.RemoveSelectedItem();
    }

    public void UpdateCanClipboardCommand() => insertImageFromClipboardCommand?.NotifyCanExecuteChanged();
    private bool IsClipboardContainsImage() => clipboard.ContainsImage();

    [RelayCommand(CanExecute = nameof(IsClipboardContainsImage))]
    private void InsertImageFromClipboard()
    {
        if (clipboard.GetImage() is { } image)
            ImageProvider.InsertImage(ImageProvider.SelectedImageIndex + 1, image);
    }

    [RelayCommand]
    private void CopyToClipboard(CaptureImage? img)
    {
        if (img is not null)
            clipboard.SetImage(img.TransformedImage);
    }

    void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
    {
        removeSelectedImageCommand?.NotifyCanExecuteChanged();
    }
}
