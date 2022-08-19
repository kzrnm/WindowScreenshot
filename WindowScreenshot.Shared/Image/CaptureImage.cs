using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;

public partial class CaptureImage : ObservableObject
{
    public ImageRatioSize ImageRatioSize { get; }


    [ObservableProperty]
    private ImageKind _ImageKind;

    [ObservableProperty]
    private bool _IsSideCutMode;

    [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
    partial void OnImageKindChanged(ImageKind value) => UpdateTransformedImage();
    [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
    partial void OnIsSideCutModeChanged(bool value) => UpdateTransformedImage();


    private void OnImageRatioSizePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ImageRatioSize.HeightPercentage):
            case nameof(ImageRatioSize.WidthPercentage):
                UpdateTransformedImage();
                break;
        }
    }
    private void UpdateTransformedImage()
    {
        var bitmap = ImageSource;
        if (IsSideCutMode)
        {
            bitmap =
               new CroppedBitmap(ImageSource,
               new Int32Rect(ImageSource.PixelWidth / 8, 0, ImageSource.PixelWidth * 3 / 4, ImageSource.PixelHeight));
            bitmap.Freeze();
        }
        if (ImageRatioSize.WidthPercentage != 100
            || ImageRatioSize.HeightPercentage != 100)
        {
            bitmap =
                new TransformedBitmap(bitmap,
                new ScaleTransform(ImageRatioSize.WidthPercentage / 100, ImageRatioSize.HeightPercentage / 100));
            bitmap.Freeze();
        }
        TransformedImage = bitmap;
    }

    [ObservableProperty]
    private BitmapSource _TransformedImage;

    public BitmapSource ImageSource { get; }
    public string? SourcePath { get; }
    private ImageKind OrigKind { get; }

    public static int DefaultJpegQualityLevel { set; get; } = 100;
    public int JpegQualityLevel { get; set; } = DefaultJpegQualityLevel;

    [MemberNotNullWhen(returnValue: true, member: nameof(SourcePath))]
    public bool CanUseFileStream
        => SourcePath != null
            && ImageRatioSize.IsNotChanged
            && !IsSideCutMode
            && ImageKind == OrigKind;

    public CaptureImage(BitmapSource source) : this(source, null) { }
    public CaptureImage(BitmapSource source, string? sourcePath)
    {
        ImageSource = source;
        _TransformedImage = source;

        SourcePath = sourcePath;
        ImageRatioSize = new ImageRatioSize(source);
        IsSideCutMode = false;
        if (sourcePath is null)
        {
            ImageKind = ImageKind.Jpg;
        }
        else
        {
            ImageKind = OrigKind = Regex.IsMatch(Path.GetExtension(sourcePath), @"\.jpe?g", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
                ? ImageKind.Jpg
                : ImageKind.Png;
        }

        ImageRatioSize.PropertyChanged += OnImageRatioSizePropertyChanged;
    }

    private BitmapEncoder GetEncoder()
        => ImageKind switch
        {
            ImageKind.Jpg => new JpegBitmapEncoder { QualityLevel = JpegQualityLevel },
            ImageKind.Png => new PngBitmapEncoder { Interlace = PngInterlaceOption.Off },
            _ => throw new InvalidOperationException($"invalid {nameof(ImageKind)}"),
        };

    private byte[] ToStreamImpl()
    {
        BitmapEncoder encoder = GetEncoder();
        encoder.Frames.Add(BitmapFrame.Create(TransformedImage));

        using var ms = new MemoryStream();
        encoder.Save(ms);
        return ms.ToArray();
    }

    public byte[] ToByteArray() => CanUseFileStream ? File.ReadAllBytes(SourcePath) : ToStreamImpl();
}
