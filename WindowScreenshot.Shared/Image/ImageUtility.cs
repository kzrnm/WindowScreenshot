using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;
public class ImageUtility
{
    /// <summary>
    /// 画像を <see cref="byte[]"/> に変換
    /// </summary>
    public static byte[] ImageToByteArray(BitmapSource source, BitmapEncoder encoder)
    {
        using var ms = ImageToStream(source, encoder);
        return ms.ToArray();
    }

    /// <summary>
    /// 画像を PNG 形式の <see cref="MemoryStream"/> に変換
    /// </summary>
    public static MemoryStream ImageToStream(BitmapSource source)
    {
        return ImageToStream(source, new PngBitmapEncoder { Interlace = PngInterlaceOption.Off });
    }


    /// <summary>
    /// 画像を <see cref="MemoryStream"/> に変換
    /// </summary>
    public static MemoryStream ImageToStream(BitmapSource source, BitmapEncoder encoder)
    {
        encoder.Frames.Add(BitmapFrame.Create(source));
        var ms = new MemoryStream();
        encoder.Save(ms);
        return ms;
    }

    /// <summary>
    /// ファイルから画像を読み込む
    /// 失敗したらnullを返します
    /// </summary>
    /// <param name="filePath">読み込むファイル</param>
    public static CaptureImage? GetImageFromFile(string filePath)
        => GetImageFromBinary(File.ReadAllBytes(filePath)) switch
        {
            null => null,
            var bmp => new(bmp, filePath),
        };


    /// <summary>
    /// 画像が格納された <see cref="byte[]"/> を読み込んで <see cref="BitmapSource"/> を返します
    /// </summary>
    public static BitmapSource? GetImageFromBinary(byte[] bytes)
    {
        using var ms = new MemoryStream(bytes);
        return GetImageFromBinary(ms);
    }
    /// <summary>
    /// 画像が格納された <see cref="MemoryStream"/> を読み込んで <see cref="BitmapSource"/> を返します
    /// </summary>
    public static BitmapSource? GetImageFromBinary(MemoryStream ms)
    {
        try
        {
            var image = new WriteableBitmap(BitmapFrame.Create(ms));
            image.Freeze();
            return image;
        }
        catch (NotSupportedException e) when (e.InnerException is COMException)
        {
            return null;
        }
    }
}
