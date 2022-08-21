﻿using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.EventHandlerHistory;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.DragDrop;
using System.Windows;

namespace Kzrnm.WindowScreenshot.ViewModels;

public class WindowCapturerViewModelTest
{
    public WeakReferenceMessenger Messenger;
    public ImageProvider ImageProvider;

    public WindowCapturerViewModelTest()
    {
        Messenger = new();
        ImageProvider = new ImageProvider(Messenger, new CaptureImageService());
    }

    [Fact]
    public void ImageVisibility()
    {
        var viewModel = new WindowCapturerViewModel(Messenger, new ImageDropTarget.Factory(ImageProvider), ImageProvider);
        using (var ph = new PropertyChangedHistory(viewModel))
        {
            ph.Should().Equal(new Dictionary<string, int> { });
            viewModel.ImageVisibility.Should().Be(Visibility.Collapsed);
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(2, 2));
            viewModel.ImageVisibility.Should().Be(Visibility.Visible);
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(2, 2));
            viewModel.ImageVisibility.Should().Be(Visibility.Visible);
            ph.Should().Equal(new Dictionary<string, int>
            {
                { "ImageVisibility", 1 }
            });
            ImageProvider.Images.Clear();
            viewModel.ImageVisibility.Should().Be(Visibility.Collapsed);
            ph.Should().Equal(new Dictionary<string, int>
            {
                { "ImageVisibility", 2 }
            });
        }
        using (var ph = new PropertyChangedHistory(viewModel))
        {
            ph.Should().Equal(new Dictionary<string, int> { });
            viewModel.AlwaysImageArea = true;
            viewModel.ImageVisibility.Should().Be(Visibility.Visible);
            ph.Should().Equal(new Dictionary<string, int>
            {
                { "AlwaysImageArea", 1 },
                { "ImageVisibility", 1 }
            });
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(2, 2));
            viewModel.ImageVisibility.Should().Be(Visibility.Visible);
            ph.Should().Equal(new Dictionary<string, int>
            {
                { "AlwaysImageArea", 1 },
                { "ImageVisibility", 1 }
            });
        }
    }
}
