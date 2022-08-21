using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image.DragDrop;

internal class CaptureImageMetadata
{
    public string? SourcePath { get; set; }
    public ImageKind OrigKind { get; set; }
    public ImageKind ImageKind { get; set; }
    public bool IsSideCutMode { get; set; }
    public int JpegQualityLevel { get; set; }
    public SerializableImageRatioSize ImageRatioSize { get; set; } = new();

    public static CaptureImageMetadata FromCaptureImage(CaptureImage captureImage)
    {
        return new CaptureImageMetadata
        {
            SourcePath = captureImage.SourcePath,
            OrigKind = captureImage.OrigKind,
            ImageKind = captureImage.ImageKind,
            IsSideCutMode = captureImage.IsSideCutMode,
            JpegQualityLevel = captureImage.JpegQualityLevel,
            ImageRatioSize =
                {
                    Width = captureImage.ImageRatioSize.Width,
                    Height = captureImage.ImageRatioSize.Height,
                },
        };
    }

    public CaptureImage ToCaptureImage(BitmapSource img)
    {
        var result = new CaptureImage(img, SourcePath, OrigKind)
        {
            ImageKind = ImageKind,
            IsSideCutMode = IsSideCutMode,
            JpegQualityLevel = JpegQualityLevel,
        };
        result.ImageRatioSize.Width = ImageRatioSize.Width;
        result.ImageRatioSize.Height = ImageRatioSize.Height;
        return result;
    }
}

internal class SerializableImageRatioSize
{
    public int Width { get; set; }
    public int Height { get; set; }
}
