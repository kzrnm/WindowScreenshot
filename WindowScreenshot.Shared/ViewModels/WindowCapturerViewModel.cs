using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Kzrnm.WindowScreenshot.Image;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class WindowCapturerViewModel : ObservableRecipient, IRecipient<ImageCountChangedMessage>
{
    public ImageProvider ImageProvider { get; }
    public WindowCapturerViewModel(ICaptureImageService captureImageService, ImageProvider imageProvider)
        : this(WeakReferenceMessenger.Default, captureImageService, imageProvider)
    { }
    public WindowCapturerViewModel(IMessenger messenger, ICaptureImageService captureImageService, ImageProvider imageProvider)
        : base(messenger)
    {
        DropHandler = new ImageDropTarget(captureImageService, imageProvider, true);
        ImageProvider = imageProvider;
        IsActive = true;
    }

    public ImageDropTarget DropHandler { get; }

    [ObservableProperty]
    private bool _AlwaysImageArea;

    [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
    partial void OnAlwaysImageAreaChanged(bool value)
    {
        UpdateImageVisibility();
    }

    private void UpdateImageVisibility()
    {
        ImageVisibility = (_AlwaysImageArea || ImageProvider.Images.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
    }

    [ObservableProperty]
    private Visibility _ImageVisibility = Visibility.Collapsed;

    public void OnWindowClosing() => Messenger.Send<ImageClearRequestMessage>();
    void IRecipient<ImageCountChangedMessage>.Receive(ImageCountChangedMessage message)
    {
        UpdateImageVisibility();
    }
}
