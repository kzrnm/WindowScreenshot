using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Windows;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class ImageListViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
{
    private readonly IClipboardManager ClipboardManager;
    public ImageProvider ImageProvider { get; }
    public ImageListViewModel(ImageDropTarget.Factory imageDropTargetFactory, IClipboardManager clipboardManager, ImageProvider imageProvider)
        : this(WeakReferenceMessenger.Default, imageDropTargetFactory, clipboardManager, imageProvider)
    { }
    public ImageListViewModel(IMessenger messenger, ImageDropTarget.Factory imageDropTargetFactory, IClipboardManager clipboardManager, ImageProvider imageProvider)
        : base(messenger)
    {
        ImageProvider = imageProvider;
        ClipboardManager = clipboardManager;
        DropHandler = imageDropTargetFactory.Build(false);
        DragHandler = new();
        IsActive = true;
    }

    public ImageDropTarget DropHandler { get; }
    public ImageDragSource DragHandler { get; }

    private bool IsSelectedImage() => ImageProvider.SelectedImageIndex >= 0;
    [RelayCommand(CanExecute = nameof(IsSelectedImage))]
    private void RemoveSelectedImage()
    {
        ImageProvider.Images.RemoveSelectedItem();
    }

    public void UpdateCanClipboardCommand() => insertImageFromClipboardCommand?.NotifyCanExecuteChanged();
    private bool IsClipboardContainsImage() => ClipboardManager.ContainsImage();

    [RelayCommand(CanExecute = nameof(IsClipboardContainsImage))]
    private void InsertImageFromClipboard()
    {
        if (ClipboardManager.GetImage() is { } image)
            ImageProvider.InsertImage(ImageProvider.SelectedImageIndex + 1, image);
    }

    [RelayCommand]
    private void CopyToClipboard(CaptureImage? img)
    {
        if (img is not null)
            ClipboardManager.SetImage(img.TransformedImage);
    }

    void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
    {
        removeSelectedImageCommand?.NotifyCanExecuteChanged();
    }
}
