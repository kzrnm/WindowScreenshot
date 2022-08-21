using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;

 partial class CaptureImage
{
    internal class SerializableCaptureImage
    {
        public byte[] Image { get; set; } = null!;
        public string? SourcePath { get; set; }
        public ImageKind OrigKind { get; set; }
        public ImageKind ImageKind { get; set; }
        public bool IsSideCutMode { get; set; }
        public int JpegQualityLevel { get; set; }
        public SerializableImageRatioSize ImageRatioSize { get; set; } = new();

        public static SerializableCaptureImage FromCaptureImage(CaptureImage captureImage)
        {
            return new SerializableCaptureImage
            {
                Image = ImageUtility.ImageToByteArray(captureImage.ImageSource, new PngBitmapEncoder { Interlace = PngInterlaceOption.Off }),
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

        public CaptureImage ToCaptureImage()
        {
            var img = ImageUtility.GetImageFromBinary(Image);
            if (img is null)
                throw new ArgumentException();

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
}
