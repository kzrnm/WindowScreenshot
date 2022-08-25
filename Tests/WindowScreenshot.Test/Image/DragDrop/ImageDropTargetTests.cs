using CommunityToolkit.Mvvm.Messaging;
using GongSolutions.Wpf.DragDrop;
using Moq;
using System.Windows;
using System.Windows.Controls;

namespace Kzrnm.WindowScreenshot.Image.DragDrop;

public class ImageDropTargetTests
{
    [UIFact]
    public async Task NotMatch()
    {
        var captureImageMock = new Mock<ICaptureImageService>();
        var imageProvider = new ImageProvider(WeakReferenceMessenger.Default);
        var dropTarget = new ImageDropTarget(imageProvider, captureImageMock.Object, false);
        var mock = new Mock<IDropInfo>();
        var elm = new ListView();
        mock.SetupGet(d => d.VisualTarget).Returns(elm);

        dropTarget.DragOver(mock.Object);
        mock.VerifySet(d => d.NotHandled = false, Times.Once());
        await dropTarget.DropImpl(mock.Object);
        mock.VerifySet(d => d.NotHandled = false, Times.Exactly(2));
    }
    [UIFact]
    public async Task NotMatchAllowOtherDrop()
    {
        var captureImageMock = new Mock<ICaptureImageService>();
        var captureImageService = captureImageMock.Object;
        var imageProvider = new ImageProvider(WeakReferenceMessenger.Default);
        var dropTarget = new ImageDropTarget(imageProvider, captureImageMock.Object, true);
        var mock = new Mock<IDropInfo>();
        var elm = new ListView();
        mock.SetupGet(d => d.VisualTarget).Returns(elm);

        dropTarget.DragOver(mock.Object);
        mock.VerifySet(d => d.NotHandled = true, Times.Once());
        await dropTarget.DropImpl(mock.Object);
        mock.VerifySet(d => d.NotHandled = true, Times.Exactly(2));
    }

    [UIFact]
    public async Task FileDrop()
    {
        var captureImageMock = new Mock<ICaptureImageService>();
        var captureImageService = captureImageMock.Object;
        var imageProvider = new ImageProvider(WeakReferenceMessenger.Default);
        var dropTarget = new ImageDropTarget(imageProvider, captureImageMock.Object, true);
        var mock = new Mock<IDropInfo>();
        var elm = new ListView();
        mock.SetupGet(d => d.VisualTarget).Returns(elm);
        mock.SetupGet(d => d.Data).Returns(CreateData());

        captureImageMock.Setup(c => c.IsImageFile(It.IsAny<string>())).Returns(true);

        dropTarget.DragOver(mock.Object);
        await dropTarget.DropImpl(mock.Object);
        mock.VerifySet(d => d.NotHandled = It.IsAny<bool>(), Times.Never());

        mock.VerifySet(d => d.Effects = DragDropEffects.Copy);
        mock.VerifySet(d => d.DropTargetAdorner = It.IsAny<Type>(), Times.Never());

        captureImageMock.Verify(c => c.GetImageFromFileAsync(@"C:\Foo\bar1.jpg"));
        captureImageMock.Verify(c => c.GetImageFromFileAsync(@"C:\Foo\bar2.jpg"));
        captureImageMock.Verify(c => c.GetImageFromFileAsync(@"C:\Foo\bar10.jpg"));
        captureImageMock.Verify(c => c.GetImageFromFileAsync(@"C:\Foo\foo2.jpg"));

        static DataObject CreateData()
        {
            var data = new DataObject();
            data.SetFileDropList(new()
            {
                @"C:\Foo\bar1.jpg",
                @"C:\Foo\foo2.jpg",
                @"C:\Foo\bar10.jpg",
                @"C:\Foo\bar2.jpg",
            });
            return data;
        }
    }

    [UIFact]
    public async Task TargetCollection()
    {
        var captureImageMock = new Mock<ICaptureImageService>();
        var captureImageService = captureImageMock.Object;
        var imageProvider = new ImageProvider(WeakReferenceMessenger.Default);
        var dropTarget = new ImageDropTarget(imageProvider, captureImageMock.Object, true);
        var mock = new Mock<IDropInfo>();
        var elm = new ListView();
        mock.SetupGet(d => d.VisualTarget).Returns(elm);
        mock.SetupGet(d => d.Data).Returns(CreateData());
        mock.SetupGet(d => d.TargetCollection).Returns(imageProvider.Images);

        captureImageMock.Setup(c => c.IsImageFile(It.IsAny<string>())).Returns(true);

        dropTarget.DragOver(mock.Object);
        await dropTarget.DropImpl(mock.Object);
        mock.VerifySet(d => d.NotHandled = It.IsAny<bool>(), Times.Never());

        mock.VerifySet(d => d.Effects = DragDropEffects.Copy);
        mock.VerifySet(d => d.DropTargetAdorner = DropTargetAdorners.Insert, Times.Once());

        captureImageMock.Verify(c => c.GetImageFromFileAsync(@"C:\Foo\bar1.jpg"));
        captureImageMock.Verify(c => c.GetImageFromFileAsync(@"C:\Foo\bar2.jpg"));
        captureImageMock.Verify(c => c.GetImageFromFileAsync(@"C:\Foo\bar10.jpg"));
        captureImageMock.Verify(c => c.GetImageFromFileAsync(@"C:\Foo\foo2.jpg"));

        static DataObject CreateData()
        {
            var data = new DataObject();
            data.SetFileDropList(new()
            {
                @"C:\Foo\bar1.jpg",
                @"C:\Foo\foo2.jpg",
                @"C:\Foo\bar10.jpg",
                @"C:\Foo\bar2.jpg",
            });
            return data;
        }
    }


    [UIFact]
    public async Task AddImage()
    {
        var captureImageMock = new Mock<ICaptureImageService>();
        var captureImageService = captureImageMock.Object;
        var imageProvider = new ImageProvider(WeakReferenceMessenger.Default);
        var dropTarget = new ImageDropTarget(imageProvider, captureImageMock.Object, true);
        var mock = new Mock<IDropInfo>();
        var elm = new ListView();
        var img = TestUtil.DummyBitmapSource(5, 3);

        mock.SetupGet(d => d.VisualTarget).Returns(elm);
        mock.SetupGet(d => d.Data).Returns(CreateData());

        captureImageMock.Setup(c => c.IsImageFile(It.IsAny<string>())).Returns(true);

        dropTarget.DragOver(mock.Object);
        await dropTarget.DropImpl(mock.Object);
        mock.VerifySet(d => d.NotHandled = It.IsAny<bool>(), Times.Never());

        mock.VerifySet(d => d.Effects = DragDropEffects.Copy);
        mock.VerifySet(d => d.DropTargetAdorner = It.IsAny<Type>(), Times.Never());

        var added = imageProvider.Images.Should().ContainSingle().Which;
        TestUtil.ImageToByteArray(added.ImageSource).Should().Equal(TestUtil.ImageToByteArray(img));

        DataObject CreateData()
        {
            var data = new DataObject();
            data.SetImage(img);
            return data;
        }
    }
}
