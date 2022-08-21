using GongSolutions.Wpf.DragDrop;
using System;
using System.Linq;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Image;

public class ImageDropTarget : DefaultDropHandler
{
    public class Factory
    {
        private ImageProvider ImageProvider { get; }
        private ICaptureImageService CaptureImageService { get; }
        public Factory(ICaptureImageService captureImageService, ImageProvider imageProvider)
        {
            CaptureImageService = captureImageService;
            ImageProvider = imageProvider;
        }

        public ImageDropTarget Build(bool allowOtherDrop) => new(CaptureImageService, ImageProvider, allowOtherDrop);
    }
    private ImageProvider ImageProvider { get; }
    private ICaptureImageService CaptureImageService { get; }
    public bool AllowOtherDrop { get; }
    public ImageDropTarget(ICaptureImageService captureImageService, ImageProvider imageProvider, bool allowOtherDrop)
    {
        CaptureImageService = captureImageService;
        ImageProvider = imageProvider;
        AllowOtherDrop = allowOtherDrop;
    }
    public override void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.VisualTarget == dropInfo.DragInfo?.VisualSource)
        {
            base.DragOver(dropInfo);
            return;
        }
        else if (dropInfo.Data is DataObject data)
        {
            if (data.GetData(DataFormats.FileDrop, false) is string[] files
                && files.All(f => CaptureImageService.IsImageFile(f))
                || data.ContainsImage())
            {
                dropInfo.Effects = DragDropEffects.Copy;
                if (dropInfo.TargetCollection == this.ImageProvider.Images)
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                return;
            }
        }
        dropInfo.NotHandled = AllowOtherDrop;
    }

    public override void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.VisualTarget == dropInfo.DragInfo?.VisualSource)
        {
            base.Drop(dropInfo);
            return;
        }
        else if (dropInfo.Data is DataObject data)
        {
            if (data.GetData(DataFormats.FileDrop, false) is string[] files)
            {
                Array.Sort(files, NaturalComparer.Default);
                if (dropInfo.InsertPosition == RelativeInsertPosition.None)
                    foreach (var file in files)
                        ImageProvider.TryAddImageFromFile(file);
                else
                    foreach (var file in files)
                        ImageProvider.TryInsertImageFromFile(dropInfo.UnfilteredInsertIndex, file);
                return;
            }
            if (data.ContainsImage())
            {
                var img = data.GetImage();
                if (dropInfo.InsertPosition == RelativeInsertPosition.None)
                    ImageProvider.AddImage(img);
                else
                    ImageProvider.InsertImage(dropInfo.UnfilteredInsertIndex, img);
                return;
            }
        }
        dropInfo.NotHandled = AllowOtherDrop;
    }
}
