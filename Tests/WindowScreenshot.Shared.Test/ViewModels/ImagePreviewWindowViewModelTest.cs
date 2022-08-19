﻿using CommunityToolkit.Mvvm.Messaging;
using KzLibraries.EventHandlerHistory;
using Moq;
using System.Windows.Media.Imaging;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Windows;

namespace Kzrnm.WindowScreenshot.ViewModels;

public class ImagePreviewWindowViewModelTest
{
    public WeakReferenceMessenger Messenger;
    public ImageProvider ImageProvider;

    public ImagePreviewWindowViewModelTest()
    {
        Messenger = new();
        ImageProvider = new ImageProvider(Messenger, new CaptureImageService());
    }

    [Fact]
    public void CopyToClipboardCommand()
    {
        ImageProvider.AddImage(TestUtil.DummyBitmapSource(10, 10));
        var img = ImageProvider.Images[0];
        img.ImageRatioSize.WidthPercentage = 200;
        var clipboardMock = new Mock<IClipboardManager>();
        var viewModel = new ImagePreviewWindowViewModel(Messenger, new CaptureImageService(), clipboardMock.Object, ImageProvider);

        BitmapSource? called = null;
        clipboardMock.Setup(c => c.SetImage(It.IsAny<BitmapSource>())).Callback<BitmapSource>(img => called = img);

        viewModel.CopyToClipboardCommand.Execute(img);
        clipboardMock.Verify(c => c.SetImage(It.IsAny<BitmapSource>()), Times.Once());
        called.Should().Be(img.TransformedImage);
    }

    [Fact]
    public void PasteImageFromClipboardCommand()
    {
        var img = TestUtil.DummyBitmapSource(10, 10);
        var clipboardMock = new Mock<IClipboardManager>();
        var viewModel = new ImagePreviewWindowViewModel(Messenger, new CaptureImageService(), clipboardMock.Object, ImageProvider);

        viewModel.PasteImageFromClipboardCommand.CanExecute(null).Should().BeFalse();
        viewModel.PasteImageFromClipboardCommand.Execute(null);
        ImageProvider.Images.Should().BeEmpty();

        clipboardMock.Setup(m => m.ContainsImage()).Returns(true);
        clipboardMock.Setup(m => m.GetImage()).Returns(img);

        viewModel.UpdateCanClipboardCommand();
        viewModel.PasteImageFromClipboardCommand.CanExecute(null).Should().BeTrue();
        viewModel.PasteImageFromClipboardCommand.Execute(null);
        ImageProvider.Images.Should().ContainSingle();
    }

    [Fact]
    public void SelectedImage()
    {
        var clipboardMock = new Mock<IClipboardManager>();
        var viewModel = new ImagePreviewWindowViewModel(Messenger, new CaptureImageService(), clipboardMock.Object, ImageProvider);
        using var ph = new PropertyChangedHistory(viewModel);

        viewModel.SelectedImage.Should().BeNull();
        ph.Should().Equal(new Dictionary<string, int> { });
        ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
        viewModel.SelectedImage.Should().NotBeNull();
        ph.Should().Equal(new Dictionary<string, int>
            {
                { "Visibility",1 },
                { "SelectedImage",1 },
            });
        ImageProvider.Images.Clear();
        viewModel.SelectedImage.Should().BeNull();
        ph.Should().Equal(new Dictionary<string, int>
            {
                { "Visibility",2 },
                { "SelectedImage",2 },
            });
    }

    [Fact]
    public void Visibility()
    {
        var clipboardMock = new Mock<IClipboardManager>();
        var viewModel = new ImagePreviewWindowViewModel(Messenger, new CaptureImageService(), clipboardMock.Object, ImageProvider);
        using var ph = new PropertyChangedHistory(viewModel);

        viewModel.Visibility.Should().Be(System.Windows.Visibility.Collapsed);
        ph.Should().Equal(new Dictionary<string, int> { });
        ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
        viewModel.Visibility.Should().Be(System.Windows.Visibility.Visible);
        ph.Should().Equal(new Dictionary<string, int>
            {
                { "Visibility",1 },
                { "SelectedImage",1 },
            });
        ImageProvider.Images.Clear();
        viewModel.Visibility.Should().Be(System.Windows.Visibility.Collapsed);
        ph.Should().Equal(new Dictionary<string, int>
            {
                { "Visibility",2 },
                { "SelectedImage",2 },
            });
    }

    [Fact]
    public void ClearImageCommand()
    {
        var clipboardMock = new Mock<IClipboardManager>();
        var viewModel = new ImagePreviewWindowViewModel(Messenger, new CaptureImageService(), clipboardMock.Object, ImageProvider);
        ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
        viewModel.ClearImageCommand.Execute(null);
        ImageProvider.Images.Should().BeEmpty();
    }
}