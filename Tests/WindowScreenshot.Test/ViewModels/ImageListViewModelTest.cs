using CommunityToolkit.Mvvm.Messaging;
using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Windows;
using Moq;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.ViewModels;

public class ImageListViewModelTest
{
    public WeakReferenceMessenger Messenger;
    public ImageProvider ImageProvider;

    public ImageListViewModelTest()
    {
        Messenger = new();
        ImageProvider = new ImageProvider(Messenger, new CaptureImageService());
    }

    [Fact]
    public void RemoveSelectedImageCommand()
    {
        var clipboardMock = new Mock<IClipboardManager>();
        var viewModel = new
        ImageListViewModel(Messenger, clipboardMock.Object, ImageProvider);

        viewModel.RemoveSelectedImageCommand.CanExecute(null).Should().BeFalse();

        ImageProvider.AddImage(TestUtil.DummyBitmapSource(10, 10));
        viewModel.RemoveSelectedImageCommand.CanExecute(null).Should().BeTrue();
        viewModel.RemoveSelectedImageCommand.Execute(null);
        ImageProvider.Images.SelectedItem.Should().BeNull();
        ImageProvider.Images.Should().BeEmpty();
    }


    [Fact]
    public void InsertImageFromClipboardCommand()
    {
        var clipboardMock = new Mock<IClipboardManager>();
        var viewModel = new
        ImageListViewModel(Messenger, clipboardMock.Object, ImageProvider);
        viewModel.InsertImageFromClipboardCommand.CanExecute(null).Should().BeFalse();

        viewModel.InsertImageFromClipboardCommand.CanExecute(null).Should().BeFalse();
        viewModel.InsertImageFromClipboardCommand.Execute(null);
        ImageProvider.Images.Should().BeEmpty();

        NotifyCollectionChangedEventArgs? lastCollectionChanged = null;
        ImageProvider.Images.CollectionChanged += (_, e) => lastCollectionChanged = e;

        clipboardMock.Setup(m => m.ContainsImage()).Returns(true);
        clipboardMock.Setup(m => m.GetImage()).Returns(() => TestUtil.DummyBitmapSource(10, 10));

        viewModel.InsertImageFromClipboardCommand.CanExecute(null).Should().BeTrue();
        viewModel.InsertImageFromClipboardCommand.Execute(null);
        lastCollectionChanged?.NewStartingIndex.Should().Be(0);

        viewModel.InsertImageFromClipboardCommand.CanExecute(null).Should().BeTrue();
        viewModel.InsertImageFromClipboardCommand.Execute(null);
        lastCollectionChanged?.NewStartingIndex.Should().Be(1);

        ImageProvider.Images.SelectedIndex = 0;
        viewModel.InsertImageFromClipboardCommand.CanExecute(null).Should().BeTrue();
        viewModel.InsertImageFromClipboardCommand.Execute(null);
        lastCollectionChanged?.NewStartingIndex.Should().Be(1);
    }

    [Fact]
    public void CopyToClipboardCommand()
    {
        ImageProvider.AddImage(TestUtil.DummyBitmapSource(10, 10));
        var img = ImageProvider.Images[0];
        img.ImageRatioSize.WidthPercentage = 200;
        var clipboardMock = new Mock<IClipboardManager>();
        var viewModel = new
        ImageListViewModel(Messenger, clipboardMock.Object, ImageProvider);

        BitmapSource? called = null;
        clipboardMock.Setup(c => c.SetImage(It.IsAny<BitmapSource>())).Callback<BitmapSource>(img => called = img);

        viewModel.CopyToClipboardCommand.Execute(img);
        clipboardMock.Verify(c => c.SetImage(It.IsAny<BitmapSource>()), Times.Once());
        called.Should().Be(img.TransformedImage);
    }


    [UIFact]
    public void DropTargetSameVisualSource()
    {
        var captureImageMock = new Mock<ICaptureImageService>();
        var captureImageService = captureImageMock.Object;
        var imageProvider = new ImageProvider(WeakReferenceMessenger.Default, captureImageService);
        var dropTarget = new ImageListViewModel.DropTarget(imageProvider);
        var mock = new Mock<IDropInfo>();

        var elm = new ListView();
        mock.SetupGet(d => d.VisualTarget).Returns(elm);

        var dragInfoMock = new Mock<IDragInfo>();
        dragInfoMock.SetupGet(d => d.VisualSource).Returns(elm);
        mock.SetupGet(d => d.DragInfo).Returns(dragInfoMock.Object);

        dropTarget.DragOver(mock.Object);
        dropTarget.Drop(mock.Object);
        mock.VerifySet(d => d.NotHandled = It.IsAny<bool>(), Times.Never());
    }
}
