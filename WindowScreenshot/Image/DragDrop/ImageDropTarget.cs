using GongSolutions.Wpf.DragDrop;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image.DragDrop;

public class ImageDropTarget : IDropTarget
{
    public record Factory(ICaptureImageService CaptureImageService, ImageProvider ImageProvider)
    {
        public ImageDropTarget Build(bool allowOtherDrop) => new(ImageProvider, CaptureImageService, allowOtherDrop);
    }

    private ICaptureImageService CaptureImageService { get; }
    private ImageProvider ImageProvider { get; }
    public bool AllowOtherDrop { get; }
    public ImageDropTarget(ImageProvider imageProvider, ICaptureImageService captureImageService, bool allowOtherDrop)
    {
        ImageProvider = imageProvider;
        CaptureImageService = captureImageService;
        AllowOtherDrop = allowOtherDrop;
    }
    public virtual void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not DataObject data)
        {
            dropInfo.NotHandled = AllowOtherDrop;
            return;
        }
        if (IsAcceptable(data))
        {
            dropInfo.Effects = DragDropEffects.Copy;
            if (dropInfo.TargetCollection == ImageProvider.Images)
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
        }

        static bool IsAcceptable(DataObject data)
        {
            return data.GetDataPresent(DataFormats.FileDrop)
                || data.ContainsImage();
        }
    }

    public virtual async void Drop(IDropInfo dropInfo)
        => await DropImpl(dropInfo).ConfigureAwait(false);
    internal async ValueTask DropImpl(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not DataObject data)
        {
            dropInfo.NotHandled = AllowOtherDrop;
            return;
        }

        if (data.ContainsCaptureImage() && data.GetCaptureImage() is { } captureImage)
        {
            AddImage(dropInfo, captureImage);
        }
        else if (data.GetDataPresent(DataFormats.FileDrop) && data.GetData(DataFormats.FileDrop, true) is string[] files)
        {
            Array.Sort(files, NaturalComparer.Default);
            await AddImagesFromFiles(dropInfo, files).ConfigureAwait(false);
        }
        else if (data.ContainsImage() && data.GetImage() is { } bitmap)
        {
            AddImage(dropInfo, bitmap);
        }
        else
            dropInfo.NotHandled = AllowOtherDrop;
    }

    private void AddImage(IDropInfo dropInfo, CaptureImage image)
    {
        if (dropInfo.InsertPosition == RelativeInsertPosition.None)
            ImageProvider.Images.Add(image);
        else
            ImageProvider.Images.Insert(dropInfo.UnfilteredInsertIndex, image);
    }

    private void AddImage(IDropInfo dropInfo, BitmapSource bmp)
        => AddImage(dropInfo, new CaptureImage(bmp));

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code", "CAC002:ConfigureAwaitChecker", Justification = "UI thread")]
    private async Task AddImagesFromFiles(IDropInfo dropInfo, string[] files)
    {
        var images = await Task.WhenAll(files.Select(f => CaptureImageService.GetImageFromFileAsync(f))).ConfigureAwait(true);
        foreach (var image in images)
            if (image is not null)
                AddImage(dropInfo, image);
    }
}
