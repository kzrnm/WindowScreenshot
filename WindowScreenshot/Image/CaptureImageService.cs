using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kzrnm.WindowScreenshot.Image;
public interface ICaptureImageService
{
    /// <summary>
    /// ファイルから画像を読み込む
    /// 失敗したらnullを返す
    /// </summary>
    /// <param name="filePath">読み込むファイル</param>
    Task<CaptureImage?> GetImageFromFileAsync(string filePath);
    bool IsImageFile(string fileName);
    bool IsJpegFile(string fileName);
}
public class CaptureImageService : ICaptureImageService
{
    public Task<CaptureImage?> GetImageFromFileAsync(string filePath)
        => ImageUtility.GetCaptureImageFromFileAsync(filePath);

    public bool IsImageFile(string fileName)
    {
        var ext = Path.GetExtension(fileName);
        if (ext.Length <= 1) return false;
        ext = ext[1..];
        return ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase)
            || ext.Equals("jpg", StringComparison.OrdinalIgnoreCase)
            || ext.Equals("png", StringComparison.OrdinalIgnoreCase)
            || ext.Equals("bmp", StringComparison.OrdinalIgnoreCase);
    }
    public static readonly Regex jpegPattern
        = new(@"\.jpe?g", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    public bool IsJpegFile(string fileName)
    {
        var ext = Path.GetExtension(fileName);
        if (ext.Length <= 1) return false;
        ext = ext[1..];
        return
            ext.Equals("jpg", StringComparison.OrdinalIgnoreCase)
            || ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase);
    }
}