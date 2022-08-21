using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;

public class ImageDragSource : DefaultDragHandler
{
    //public override void StartDrag(IDragInfo dragInfo)
    //{
    //    base.StartDrag(dragInfo);
    //    if (dragInfo.Data is CaptureImage image)
    //    {

    //        var dataObj = new DataObject();
    //        dataObj.SetData(typeof(BitmapSource), image.TransformedImage);

    //        var pngEnc = new PngBitmapEncoder();
    //        pngEnc.Frames.Add(BitmapFrame.Create(image.TransformedImage));
    //        using var ms = new System.IO.MemoryStream();
    //        pngEnc.Save(ms);
    //        dataObj.SetData("PNG", ms);

    //        //dataObj.SetImage(image.TransformedImage);
    //        dragInfo.DragDropHandler(dragInfo.VisualSource, dataObj, DragDropEffects.Copy);
    //    }
    //}
}
