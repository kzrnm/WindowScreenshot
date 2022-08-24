using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

    private BitmapSource _TransformedImage;
    public BitmapSource TransformedImage
    {
        private set => SetProperty(ref _TransformedImage, value);
        get => _TransformedImage;
    }

    public BitmapSource ImageSource { get; }
    public string? SourcePath { get; }
    public ImageKind OrigKind { get; }
    public static int DefaultJpegQualityLevel { set; get; } = 100;
    public int JpegQualityLevel { get; set; } = DefaultJpegQualityLevel;

    public CaptureImage(BitmapSource source) : this(source, null) { }
    public CaptureImage(BitmapSource source, string? sourcePath) : this(source, sourcePath, GetKind(sourcePath)) { }
    public CaptureImage(BitmapSource source, string? sourcePath, ImageKind origKind)
    {
        ImageSource = source;
        ImageRatioSize = new ImageRatioSize(source);
        SourcePath = sourcePath;
        _TransformedImage = source;

        _ImageKind = OrigKind = origKind;
        _IsSideCutMode = false;

        ImageRatioSize.PropertyChanged += OnImageRatioSizePropertyChanged;
    }

    private static ImageKind GetKind(string? sourcePath)
    {
        if (sourcePath is null)
            return ImageKind.Jpg;

        var ext = Path.GetExtension(sourcePath).ToLowerInvariant();
        if (ext is ".jpeg" or ".jpg") return ImageKind.Jpg;
        return ImageKind.Png;
    }

    private BitmapEncoder GetEncoder()
        => ImageKind switch
        {
            ImageKind.Jpg => new JpegBitmapEncoder { QualityLevel = JpegQualityLevel },
            ImageKind.Png => new PngBitmapEncoder { Interlace = PngInterlaceOption.Default },
            _ => throw new InvalidOperationException($"invalid {nameof(ImageKind)}"),
        };

    private Stream ToStreamImpl()
        => ImageUtility.ImageToStream(TransformedImage, GetEncoder());

    [MemberNotNullWhen(returnValue: true, member: nameof(SourcePath))]
    private bool CanUseFileStream()
        => SourcePath != null
            && ImageRatioSize.IsNotChanged
            && !IsSideCutMode
            && ImageKind == OrigKind
            && File.Exists(SourcePath);
    public Stream ToStream() => CanUseFileStream() ? new FileStream(SourcePath, FileMode.Open, FileAccess.Read) : ToStreamImpl();
}
