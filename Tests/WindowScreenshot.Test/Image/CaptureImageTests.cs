namespace Kzrnm.WindowScreenshot.Image;

public class CaptureImageTests
{
    [Fact]
    public void ConstructorTest()
    {
        new CaptureImage(TestUtil.DummyBitmapSource(5, 5, 227)).JpegQualityLevel.Should().Be(100);
        CaptureImage.DefaultJpegQualityLevel = 85;
        new CaptureImage(TestUtil.DummyBitmapSource(5, 5, 227)).JpegQualityLevel.Should().Be(85);
    }

    public static readonly TheoryData ToByteArrayJpegTest_Data = new TheoryData<CaptureImage, byte[]>()
    {
        {
            new CaptureImage(TestUtil.DummyBitmapSource(5, 5, 227)) { JpegQualityLevel = 100 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }
        },
        {
            new CaptureImage(TestUtil.DummyBitmapSource(5, 5, 227)) { JpegQualityLevel = 10 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }
        },
    };

    [Theory]
    [MemberData(nameof(ToByteArrayJpegTest_Data))]
    public void ToByteArrayJpegTest(CaptureImage ci, byte[] startsWith)
    {
        using var ms = new MemoryStream();
        using (var stream = ci.ToStream())
            stream.CopyTo(ms);
        ms.Position = 0;
        ms.ToArray()
            .Should()
            .StartWith(startsWith);
    }


    public static readonly TheoryData ToByteArrayPngTest_Data = new TheoryData<CaptureImage, byte[], int>()
    {
        {
            new CaptureImage(TestUtil.DummyBitmapSource(5, 5, 227)) { ImageKind = ImageKind.Png, JpegQualityLevel = 10 },
            new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52, },
            223
        },
    };

    [Theory]
    [MemberData(nameof(ToByteArrayPngTest_Data))]
    public void ToByteArrayPngTest(CaptureImage ci, byte[] startsWith, int length)
    {
        using var ms = new MemoryStream();
        using var stream = ci.ToStream();
        stream.CopyTo(ms);
        ms.Position = 0;
        ms.ToArray()
            .Should()
            .StartWith(startsWith)
            .And
            .HaveCount(length);
    }
}
