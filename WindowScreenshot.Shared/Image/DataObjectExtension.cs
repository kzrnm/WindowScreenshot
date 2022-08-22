using Kzrnm.WindowScreenshot.Image.DragDrop;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;
public static class DataObjectExtension
{
    public static DataObject SetCaptureImage(this DataObject data, CaptureImage img)
    {
        var ms = new MemoryStream();
        JsonSerializer.Serialize(ms, CaptureImageMetadata.FromCaptureImage(img));
        data.SetData(typeof(CaptureImageMetadata), ms);
        data.SetImageSafe(img.ImageSource);
        return data;
    }
    public static bool ContainsCaptureImage(this DataObject data)
    {
        return data.GetDataPresent(typeof(CaptureImageMetadata)) && data.ContainsImage();
    }
    public static CaptureImage? GetCaptureImage(this DataObject data)
    {
        if (data.GetData(typeof(CaptureImageMetadata)) is not MemoryStream ms)
            return null;
        if (data.GetImageSafe() is not { } bitmap)
            return null;
        ms.Position = 0;
        return JsonSerializer.Deserialize<CaptureImageMetadata>(ms)?.ToCaptureImage(bitmap);
    }

    /// <summary>
    /// <paramref name="source"/> を PNG 形式と Bitmap 形式で <paramref name="data"/> に格納します。
    /// </summary>
    public static DataObject SetImageSafe(this DataObject data, BitmapSource source)
    {
        data.SetData("PNG", ImageUtility.ImageToStream(source));
        data.SetImage(source);
        return data;
    }

    /// <summary>
    /// <paramref name="data"/> から PNG 形式または Bitmap 形式で取得します。
    /// </summary>
    public static BitmapSource? GetImageSafe(this IDataObject data)
    {
        if (data.GetDataPresent("PNG") && data.GetData("PNG") is MemoryStream pngMemoryStream)
        {
            if (ImageUtility.GetImageFromStream(pngMemoryStream) is { } result)
                return result;
        }

        if (data.GetDataPresent(typeof(BitmapSource)) && data.GetData(typeof(BitmapSource)) is BitmapSource source)
        {
            if (data.GetDataPresent(DataFormats.Dib) && data.GetData(DataFormats.Dib) is MemoryStream dib)
            {
                // メタデータを正しく読めないので無理やり修正する
                // https://github.com/kzrnm/WindowScreenshot/issues/1
                Span<byte> buffer = stackalloc byte[15];
                dib.Read(buffer);
                if (buffer[14] < 32)
                    return new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, 0);
            }
            return source;
        }
        return null;
    }
}
