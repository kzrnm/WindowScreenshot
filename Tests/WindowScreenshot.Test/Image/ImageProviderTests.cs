using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.EventHandlerHistory;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;

public class ImageProviderTests : IDisposable, IRecipient<SelectedImageChangedMessage>
{
    public WeakReferenceMessenger Messenger;
    public ImageProvider ImageProvider;
    public List<SelectedImageChangedMessage> selectedImageChangedHistories = new();

    public ImageProviderTests()
    {
        Messenger = new();
        ImageProvider = new ImageProvider(Messenger);
        Messenger.Register(this);
    }

    void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
        => selectedImageChangedHistories.Add(message);

    [Fact]
    public void AddImageTest()
    {
        var imagesCollectionChangedHistory = new CollectionChangedHistory(ImageProvider.Images);
        ImageProvider.Images.SelectedItem.Should().BeNull();
        selectedImageChangedHistories.Should().HaveCount(0);

        ImageProvider.AddImage(new(BitmapSource.Create(
                2, 2, 96, 96,
                PixelFormats.Indexed1,
                new BitmapPalette(new[] { Colors.Transparent }),
                new byte[] { 0, 0, 0, 0 }, 1)));

        ImageProvider.Images.SelectedIndex.Should().Be(0);
        ImageProvider.Images.Should().ContainSingle();

        selectedImageChangedHistories.Should().HaveCount(1);

        ImageProvider.AddImage(
            new(BitmapSource.Create(
                4, 4, 96, 96,
                PixelFormats.Indexed1,
                new BitmapPalette(new[] { Colors.Transparent }),
                new byte[] { 0, 0, 0, 0 }, 1)
            ));

        ImageProvider.Images.SelectedIndex.Should().Be(1);
        ImageProvider.Images.Should().HaveCount(2);
        imagesCollectionChangedHistory.Should().HaveCount(2);
        imagesCollectionChangedHistory[0].Action
            .Should()
            .Be(NotifyCollectionChangedAction.Add);

        ImageProvider.Images.SelectedItem.Should().NotBeNull();
        ImageProvider.Images.SelectedItem!.ImageSource.Height.Should().Be(4);
        selectedImageChangedHistories.Should().HaveCount(2);
    }

    [Fact]
    public void SelectedImageChangedTest()
    {
        var images = new[]{
            new CaptureImage(TestUtil.DummyBitmapSource(2, 2)),
            new CaptureImage(TestUtil.DummyBitmapSource(2, 2)),
        };

        ImageProvider.Images.Add(images[0]);
        selectedImageChangedHistories.Should().HaveCount(1);
        selectedImageChangedHistories[0].Value.Should().Be(images[0]);

        ImageProvider.Images.Add(images[1]);
        selectedImageChangedHistories.Should().HaveCount(2);
        selectedImageChangedHistories[1].Value.Should().Be(images[1]);

        ImageProvider.Images.Clear();
        selectedImageChangedHistories.Should().HaveCount(3);
        selectedImageChangedHistories[2].Value.Should().BeNull();
    }

    private bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Messenger.UnregisterAll(this);
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
