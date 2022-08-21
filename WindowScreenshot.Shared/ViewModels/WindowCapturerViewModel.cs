using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Image;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class WindowCapturerViewModel : ObservableRecipient, IRecipient<ImageCountChangedMessage>
{
    public ImageProvider ImageProvider { get; }
    public WindowCapturerViewModel(ImageDropTarget.Factory imageDropTargetFactory, ImageProvider imageProvider)
        : this(WeakReferenceMessenger.Default, imageDropTargetFactory, imageProvider)
    { }
    public WindowCapturerViewModel(IMessenger messenger, ImageDropTarget.Factory imageDropTargetFactory, ImageProvider imageProvider)
        : base(messenger)
    {
        DropHandler = imageDropTargetFactory.Build(true);
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
