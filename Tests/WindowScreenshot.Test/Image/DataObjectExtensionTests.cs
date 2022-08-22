using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using Kzrnm.WindowScreenshot.Image.DragDrop;

namespace Kzrnm.WindowScreenshot.Image;

public class DataObjectExtensionTests
{
    public static readonly TheoryData SetCaptureImage_GetCaptureImage_Data = new TheoryData<CaptureImage>()
    {
        new CaptureImage(TestUtil.DummyBitmapSource(10, 10), "foo.png", ImageKind.Jpg) {
            JpegQualityLevel = 100,
            ImageKind = ImageKind.Png,
            IsSideCutMode = false,
            ImageRatioSize =
            {
                HeightPercentage = 100,
                WidthPercentage = 100,
            }
        },
        new CaptureImage(TestUtil.DummyBitmapSource(10, 10), "foo.png", ImageKind.Png) {
            JpegQualityLevel = 100,
            ImageKind = ImageKind.Png,
            IsSideCutMode = false,
            ImageRatioSize =
            {
                HeightPercentage = 100,
                WidthPercentage = 100,
            }
        },
        new CaptureImage(TestUtil.DummyBitmapSource(10, 10), "foo.png", ImageKind.Png) {
            JpegQualityLevel = 100,
            ImageKind = ImageKind.Png,
            IsSideCutMode = false,
            ImageRatioSize =
            {
                Height = 1,
                Width = 1,
            }
        },
        new CaptureImage(TestUtil.DummyBitmapSource(10, 10), "foo.png", ImageKind.Jpg) {
            JpegQualityLevel = 20,
            ImageKind = ImageKind.Png,
            IsSideCutMode = true,
            ImageRatioSize =
            {
                HeightPercentage = 100,
                WidthPercentage = 100,
            }
        },
    };

    [Theory]
    [MemberData(nameof(SetCaptureImage_GetCaptureImage_Data))]
    public void SetCaptureImage_GetCaptureImage(CaptureImage captureImage)
    {
        var data = new DataObject();
        data.SetCaptureImage(captureImage);

        var cloned = data.GetCaptureImage()!;

        cloned.ImageSource.Should().NotBeSameAs(captureImage.ImageSource);
        cloned.ImageSource.Should().BeEquivalentTo(captureImage.ImageSource, opts => opts.Using(BitmapSourceEqualityComparer.Default));
        cloned.Should().BeEquivalentTo(captureImage,
            opts => opts
            .Excluding(s => s.TransformedImage)
            .Excluding(s => s.ImageSource));
    }
}