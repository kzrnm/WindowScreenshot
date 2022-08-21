using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using Kzrnm.WindowScreenshot.Windows;
using DragDrop = GongSolutions.Wpf.DragDrop.DragDrop;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class ImageListViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
{
    private readonly IClipboardManager ClipboardManager;
    public ImageProvider ImageProvider { get; }
    public ImageListViewModel(IClipboardManager clipboardManager, ImageProvider imageProvider)
        : this(WeakReferenceMessenger.Default, clipboardManager, imageProvider)
    { }
    public ImageListViewModel(IMessenger messenger, IClipboardManager clipboardManager, ImageProvider imageProvider)
        : base(messenger)
    {
        ImageProvider = imageProvider;
        ClipboardManager = clipboardManager;
        DropHandler = new DropTarget(imageProvider);
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

    internal class DropTarget : ImageDropTarget
    {
        public DropTarget(ImageProvider imageProvider) : base(imageProvider, false)
        {
            ImageProvider = imageProvider;
        }
        public ImageProvider ImageProvider { get; }

        public override void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget == dropInfo.DragInfo?.VisualSource)
            {
                DragDrop.DefaultDropHandler.DragOver(dropInfo);
                return;
            }

            base.DragOver(dropInfo);
        }
        public override void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget == dropInfo.DragInfo?.VisualSource)
            {
                var sourceIndex = dropInfo.DragInfo.SourceIndex;
                var insertIndex = dropInfo.UnfilteredInsertIndex;
                if (sourceIndex < insertIndex)
                    --insertIndex;

                if (sourceIndex != insertIndex)
                    ImageProvider.Images.Move(sourceIndex, insertIndex);
                return;
            }

            base.Drop(dropInfo);
        }
    }
}