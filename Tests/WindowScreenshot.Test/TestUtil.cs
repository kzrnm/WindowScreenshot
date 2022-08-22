using Kzrnm.WindowScreenshot.Image;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot;
using static TestUtil;

public static class TestUtil
{
    public static BitmapSource DummyBitmapSource(int width, int height, int? seed = null)
    {
        var rnd = seed switch
        {
            int s => new Random(s),
            _ => Random.Shared,
        };
        var pixelFormat = PixelFormats.Bgra32;
        var stride = (width * pixelFormat.BitsPerPixel + 7) / 8;
        var bytes = new byte[stride * width];
        rnd.NextBytes(bytes);
        return BitmapSource.Create(
        width, height, 96, 96,
        pixelFormat,
        new BitmapPalette(new[] { Colors.Transparent }),
        bytes, stride);
    }

    public static byte[]? ImageToByteArray(BitmapSource? bmp)
    {
        if (bmp is null) return null;
        return ImageUtility.ImageToByteArray(bmp, new PngBitmapEncoder { Interlace = PngInterlaceOption.Off });
    }
}

public class BitmapSourceEqualityComparer : EqualityComparer<BitmapSource>
{
    public static new BitmapSourceEqualityComparer Default => new();
    public override bool Equals(BitmapSource? x, BitmapSource? y)
    {
        return (x, y) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            (var a, var b) => ImageToByteArray(a).AsSpan().SequenceEqual(ImageToByteArray(b)),
        };
    }

    public override int GetHashCode([DisallowNull] BitmapSource obj)
    {
        return HashCode.Combine(obj.PixelHeight, obj.PixelWidth);
    }
}