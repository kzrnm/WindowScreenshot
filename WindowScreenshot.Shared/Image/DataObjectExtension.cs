using Kzrnm.WindowScreenshot.Image.DragDrop;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;
public static class DataObjectExtension
{
    public static DataObject SetCaptureImage(this DataObject data, CaptureImage img)
    {
        var ms = new MemoryStream();
        JsonSerializer.Serialize(ms, CaptureImageMetadata.FromCaptureImage(img));
        data.SetData(typeof(CaptureImageMetadata), ms);
        data.SetImage2(img.ImageSource);
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
        if (data.GetImage2() is not { } bitmap)
            return null;
        ms.Position = 0;
        return JsonSerializer.Deserialize<CaptureImageMetadata>(ms)?.ToCaptureImage(bitmap);
    }

    /// <summary>
    /// <paramref name="source"/> を PNG 形式と Bitmap 形式で <paramref name="data"/> に格納します。
    /// </summary>
    public static DataObject SetImage2(this DataObject data, BitmapSource source)
    {
        data.SetData("PNG", ImageUtility.ImageToStream(source));
        data.SetImage(source);
        return data;
    }

    /// <summary>
    /// <paramref name="data"/> から PNG 形式または Bitmap 形式で取得します。
    /// </summary>
    public static BitmapSource? GetImage2(this IDataObject data)
    {
        if (data.GetDataPresent("PNG") && data.GetData("PNG") is MemoryStream ms)
        {
            if (ImageUtility.GetImageFromBinary(ms) is { } result)
                return result;
        }
        if (data is DataObject dataObject)
            return dataObject.GetImage();
        else
            return data.GetData(DataFormats.Bitmap) as BitmapSource;
    }
}
