using GongSolutions.Wpf.DragDrop;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Image.DragDrop;

public class ImageDragSource : DefaultDragHandler
{
    public override void StartDrag(IDragInfo dragInfo)
    {
        base.StartDrag(dragInfo);
        if (dragInfo.Data is CaptureImage image)
        {
            dragInfo.DataObject = new DataObject().SetCaptureImage(image);
            dragInfo.Effects = DragDropEffects.Copy | DragDropEffects.Move;
        }
    }
}
