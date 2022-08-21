using GongSolutions.Wpf.DragDrop;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Image.DragDrop;

public class ImageDragSource : DefaultDragHandler
{
    public override void StartDrag(IDragInfo dragInfo)
    {
        base.StartDrag(dragInfo);
        if (dragInfo.Data is CaptureImage image)
        {
            var ms = new MemoryStream();
            JsonSerializer.Serialize(ms, image);
            dragInfo.Data = ms;
            dragInfo.DataFormat = DataFormats.GetDataFormat(DragDropInfo.CaptureImageFormat);
            dragInfo.Effects = dragInfo.Data != null ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.None;
        }
    }
}
