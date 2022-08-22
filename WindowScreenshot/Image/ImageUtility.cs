using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
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
    public static CaptureImage? GetCaptureImageFromFile(string filePath)
        => GetImageFromFile(filePath) switch
        {
            null => null,
            var bmp => new(bmp, filePath),
        };

    static BitmapSource? GetImageFromFile(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
        return GetImageFromStream(stream);
    }

    /// <summary>
    /// 画像が格納された <see cref="byte[]"/> を読み込んで <see cref="BitmapSource"/> を返します
    /// </summary>
    public static BitmapSource? GetImageFromBinary(byte[] bytes)
    {
        using var ms = new MemoryStream(bytes);
        return GetImageFromStream(ms);
    }
    /// <summary>
    /// 画像が格納された <see cref="MemoryStream"/> を読み込んで <see cref="BitmapSource"/> を返します
    /// </summary>
    public static BitmapSource? GetImageFromStream(Stream stream)
    {
        try
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }
        catch (NotSupportedException e) when (e.InnerException is COMException)
        {
            return null;
        }
    }
}
