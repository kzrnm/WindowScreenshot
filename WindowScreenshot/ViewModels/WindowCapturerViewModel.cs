using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Windows;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class WindowCapturerViewModel : ObservableRecipient, IRecipient<ImageCountChangedMessage>
{
    public WindowCapturerViewModel(ImageDropTarget.Factory imageDropTargetFactory, ImageProvider imageProvider, IClipboardManager clipboardManager)
        : this(WeakReferenceMessenger.Default, imageDropTargetFactory, imageProvider, clipboardManager)
    { }
    public WindowCapturerViewModel(IMessenger messenger, ImageDropTarget.Factory imageDropTargetFactory, ImageProvider imageProvider, IClipboardManager clipboardManager)
        : base(messenger)
    {
        DropHandler = imageDropTargetFactory.Build(true);
        ImageProvider = imageProvider;
        ClipboardManager = clipboardManager;
        IsActive = true;
    }

    public ImageDropTarget DropHandler { get; }
    public ImageProvider ImageProvider { get; }
    public IClipboardManager ClipboardManager { get; }

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
