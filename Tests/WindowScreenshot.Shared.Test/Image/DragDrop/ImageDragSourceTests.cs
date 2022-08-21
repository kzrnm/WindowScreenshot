using GongSolutions.Wpf.DragDrop;
using Moq;
using System.Text.Json;
using System.Windows;

namespace Kzrnm.WindowScreenshot.Image.DragDrop;

public class ImageDragSourceTests
{
    [Fact]
    public void NotCaptureImage()
    {
        var mock = new Mock<IDragInfo>();
        mock.SetupGet(d => d.Data).Returns("string");

        new ImageDragSource().StartDrag(mock.Object);

        mock.VerifySet(d => d.DataFormat = It.IsAny<DataFormat>(), Times.Never());
        mock.VerifySet(d => d.DataObject = It.IsAny<object>(), Times.Never());
    }

    [Fact]
    public void CaptureImage()
    {
        var img = new CaptureImage(TestUtil.DummyBitmapSource(2, 2)) { JpegQualityLevel = 100 };
        img.ImageRatioSize.Width = 10;

        var ms = new MemoryStream();
        JsonSerializer.Serialize(ms, img);

        var mock = new Mock<IDragInfo>();
        mock.SetupGet(d => d.Data).Returns(img);
        new ImageDragSource().StartDrag(mock.Object);

        mock.VerifySet(d => d.DataFormat = DataFormats.GetDataFormat(DragDropInfo.CaptureImageFormat), Times.Once());
        mock.VerifySet(d => d.Data = It.Is<MemoryStream>(o => Equals(ms, o)), Times.Once());
    }
    static bool Equals(MemoryStream stream, object obj)
    {
        if (obj is MemoryStream other)
        {
            return stream.ToArray().SequenceEqual(other.ToArray());
        }
        return false;
    }
}
