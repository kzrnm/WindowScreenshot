using System.Text.Json;
using System.Windows.Media.Imaging;
using Kzrnm.WindowScreenshot.Image.DragDrop;

namespace Kzrnm.WindowScreenshot.Image;

public class CaptureImageSerializeTests
{
    public static readonly TheoryData SerializeAndDeserialize_Data = new TheoryData<CaptureImage>()
    {
        new CaptureImage(TestUtil.DummyBitmapSource(2,2), "foo.bmp", ImageKind.Jpg) {
            JpegQualityLevel = 100,
            ImageKind = ImageKind.Png,
            IsSideCutMode = false,
            ImageRatioSize =
            {
                HeightPercentage = 100,
                WidthPercentage = 100,
            }
        },
        new CaptureImage(TestUtil.DummyBitmapSource(2, 2), "foo.bmp", ImageKind.Png) {
            JpegQualityLevel = 100,
            ImageKind = ImageKind.Png,
            IsSideCutMode = false,
            ImageRatioSize =
            {
                HeightPercentage = 100,
                WidthPercentage = 100,
            }
        },
        new CaptureImage(TestUtil.DummyBitmapSource(2, 2), "foo.bmp", ImageKind.Png) {
            JpegQualityLevel = 100,
            ImageKind = ImageKind.Png,
            IsSideCutMode = false,
            ImageRatioSize =
            {
                Height = 1,
                Width = 1,
            }
        },
        new CaptureImage(TestUtil.DummyBitmapSource(2,2), "foo.bmp", ImageKind.Jpg) {
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
    [MemberData(nameof(SerializeAndDeserialize_Data))]
    public void SerializeAndDeserialize(CaptureImage captureImage)
    {
        static BitmapEncoder GetEncoder() => new PngBitmapEncoder { Interlace = PngInterlaceOption.Off };
        var json = JsonSerializer.Serialize(SerializableCaptureImage.FromCaptureImage(captureImage));
        var cloned = JsonSerializer.Deserialize<SerializableCaptureImage>(json)!.ToCaptureImage();

        ImageUtility.ImageToByteArray(cloned.ImageSource, GetEncoder())
            .Should()
            .Equal(ImageUtility.ImageToByteArray(captureImage.ImageSource, GetEncoder()));

        cloned.Should().BeEquivalentTo(captureImage,
            opts => opts
            .Excluding(s => s.TransformedImage)
            .Excluding(s => s.ImageSource));
    }
}