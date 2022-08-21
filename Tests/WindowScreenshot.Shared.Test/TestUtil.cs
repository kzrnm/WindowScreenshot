using Kzrnm.WindowScreenshot.Image;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot;

public static class TestUtil
{
    public static BitmapSource DummyBitmapSource(int width, int height)
    {
        var stride = (width * PixelFormats.Indexed1.BitsPerPixel + 7) / 8;
        var bytes = new byte[stride * width];
        Random.Shared.NextBytes(bytes);
        return BitmapSource.Create(
        width, height, 96, 96,
        PixelFormats.Indexed1,
        new BitmapPalette(new[] { Colors.Transparent }),
        bytes, stride);
    }

    public static byte[] ImageToByteArray(BitmapSource bmp)
    {
        return ImageUtility.ImageToByteArray(bmp, new PngBitmapEncoder { Interlace = PngInterlaceOption.Off });
    }
}
