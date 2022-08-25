using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.WindowScreenshot.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Models;
public interface IGlobalService
{
    public ImageProvider ImageProvider { get; }
    public IObserveWindowProcess ObserveWindowProcess { get; }
    public IClipboardManager ClipboardManager { get; }

    IEnumerable<CaptureTarget> GetCaptureTargetWindows();
}
public static class GlobalServiceExtension
{
    public static bool CanPasteImageFromClipboard(this IGlobalService service) => service.ImageProvider.CanAddImage && service.ClipboardManager.ContainsImage();
    public static void PasteImageFromClipboard(this IGlobalService service)
    {
        if (service.ClipboardManager.GetImage() is { } image)
            service.ImageProvider.AddImage(new(image));
    }
    public static void CaptureScreenshot(this IGlobalService service)
    {
        var image = service.GetCaptureTargetWindows()
            .Select(service.CaptureBy)
            .OfType<BitmapSource>()
            .FirstOrDefault();

        if (image is not null)
        {
            service.ImageProvider.AddImage(new(image));
        }
    }
    private static BitmapSource? CaptureBy(this IGlobalService service, CaptureTarget captureTarget)
        => captureTarget.CaptureFrom(service.ObserveWindowProcess.CurrentWindows);
}
