using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot;

public static class TestUtil
{
    public static BitmapSource DummyBitmapSource(int width, int height)
    {
        var stride = (width * PixelFormats.Indexed1.BitsPerPixel + 7) / 8;
        return BitmapSource.Create(
        width, height, 96, 96,
        PixelFormats.Indexed1,
        new BitmapPalette(new[] { Colors.Transparent }),
        new byte[stride * width], stride);
    }

    public static byte[] ImageToByte(BitmapSource bmp)
    {
        byte[] data;
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bmp));
        using var ms = new MemoryStream();
        encoder.Save(ms);
        data = ms.ToArray();
        return data;
    }
}
