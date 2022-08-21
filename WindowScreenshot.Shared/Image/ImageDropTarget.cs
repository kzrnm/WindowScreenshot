using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;

public class ImageDropTarget : IDropTarget
{
    public class Factory
    {
        public ImageProvider ImageProvider { get; }
        public Factory(ImageProvider imageProvider)
        {
            ImageProvider = imageProvider;
        }

        public ImageDropTarget Build(bool allowOtherDrop) => new(ImageProvider, allowOtherDrop);
    }

    private ImageProvider ImageProvider { get; }
    public bool AllowOtherDrop { get; }
    public ImageDropTarget(ImageProvider imageProvider, bool allowOtherDrop)
    {
        ImageProvider = imageProvider;
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
            return data.GetDataPresent(DataFormats.FileDrop) || data.ContainsImage();
        }
    }

    public virtual void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not DataObject data)
        {
            dropInfo.NotHandled = AllowOtherDrop;
            return;
        }

        if (data.GetData(DataFormats.FileDrop, true) is string[] files)
        {
            Array.Sort(files, NaturalComparer.Default);
            if (dropInfo.InsertPosition == RelativeInsertPosition.None)
                foreach (var file in files)
                    ImageProvider.TryAddImageFromFile(file);
            else
                foreach (var file in files)
                    ImageProvider.TryInsertImageFromFile(dropInfo.UnfilteredInsertIndex, file);
        }
        else if (data.ContainsImage())
        {
            var img = data.GetImage();
            AddImage(dropInfo, img);
        }
        else
            dropInfo.NotHandled = AllowOtherDrop;
    }


    private void AddImage(IDropInfo dropInfo, BitmapSource image)
    {
        if (dropInfo.InsertPosition == RelativeInsertPosition.None)
            ImageProvider.AddImage(image);
        else
            ImageProvider.InsertImage(dropInfo.UnfilteredInsertIndex, image);
    }
}
