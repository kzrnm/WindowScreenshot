using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Windows;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class ImagePreviewWindowViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
{
    private readonly IClipboardManager clipboard;
    public ImageProvider ImageProvider { get; }
    public ImagePreviewWindowViewModel(ICaptureImageService captureImageService, IClipboardManager clipboard, ImageProvider imageProvider)
        : this(WeakReferenceMessenger.Default, captureImageService, clipboard, imageProvider)
    { }
    public ImagePreviewWindowViewModel(IMessenger messenger, ICaptureImageService captureImageService, IClipboardManager clipboard, ImageProvider imageProvider)
        : base(messenger)
    {
        DropHandler = new ImageDropTarget(captureImageService, imageProvider, false);
        ImageProvider = imageProvider;
        this.clipboard = clipboard;
        IsActive = true;
    }

    public ImageDropTarget DropHandler { get; }

    [ObservableProperty]
    private CaptureImage? _SelectedImage;
    partial void OnSelectedImageChanged(CaptureImage? value)
    {
        Visibility = value is null ? Visibility.Collapsed : Visibility.Visible;
    }

    [ObservableProperty]
    private Visibility _Visibility = Visibility.Collapsed;

    [RelayCommand]
    private void CopyToClipboard(CaptureImage? img)
    {
        if (img is not null)
            clipboard.SetImage(img.TransformedImage);
    }

    [RelayCommand]
    public void ClearImage()
    {
        ImageProvider.Images.Clear();
    }

    public void UpdateCanClipboardCommand() => pasteImageFromClipboardCommand?.NotifyCanExecuteChanged();
    private bool IsClipboardContainsImage() => clipboard.ContainsImage();
    [RelayCommand(CanExecute = nameof(IsClipboardContainsImage))]
    private void PasteImageFromClipboard()
    {
        if (clipboard.GetImage() is { } image)
            ImageProvider.AddImage(image);
    }


    void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
    {
        SelectedImage = message.Value;
    }

}
