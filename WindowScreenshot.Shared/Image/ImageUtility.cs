using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;
public class ImageUtility
{
    public static byte[] ImageToByteArray(BitmapSource source, BitmapEncoder encoder)
    {
        encoder.Frames.Add(BitmapFrame.Create(source));
        using var ms = new MemoryStream();
        encoder.Save(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// ファイルから画像を読み込む
    /// 失敗したらnullを返す
    /// </summary>
    /// <param name="filePath">読み込むファイル</param>
    public static CaptureImage? GetImageFromFile(string filePath)
        => GetImageFromBinary(File.ReadAllBytes(filePath)) switch
        {
            null => null,
            var bmp => new(bmp, filePath),
        };


    /// <summary>
    /// 画像が格納された byte 配列を読み込んで <see cref="BitmapSource"/> を返す
    /// </summary>
    public static BitmapSource? GetImageFromBinary(byte[] bytes)
    {
        using var ms = new MemoryStream(bytes);
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
