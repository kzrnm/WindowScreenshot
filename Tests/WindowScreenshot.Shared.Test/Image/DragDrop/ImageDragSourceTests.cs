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
        var img = new CaptureImage(TestUtil.DummyBitmapSource(100, 100)) { JpegQualityLevel = 100 };
        img.ImageRatioSize.Width = 10;

        var mock = new Mock<IDragInfo>();
        mock.SetupGet(d => d.Data).Returns(img);
        new ImageDragSource().StartDrag(mock.Object);

        mock.VerifySet(d => d.Effects = DragDropEffects.Copy, Times.Once());
        mock.VerifySet(d => d.DataObject = It.Is<DataObject>(o => BitmapSourceEqualityComparer.Default.Equals(o.GetImage2(), img.ImageSource)), Times.Once());
    }
}
