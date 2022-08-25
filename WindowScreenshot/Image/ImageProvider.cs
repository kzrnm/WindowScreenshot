using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowScreenshot.Image;

public partial class ImageProvider : ObservableRecipient, IRecipient<ImageClearRequestMessage>
{
    public ImageProvider() : this(WeakReferenceMessenger.Default, new SelectorObservableCollection<CaptureImage>()) { }
    public ImageProvider(IMessenger messenger) : this(messenger, new SelectorObservableCollection<CaptureImage>()) { }
    protected ImageProvider(IMessenger messenger, SelectorObservableCollection<CaptureImage> images)
        : base(messenger)
    {
        Images = images;
        IsActive = true;
        images.SelectedChanged += (_, e) => OnSelectedImageChanged(e.NewItem);
        ((INotifyPropertyChanged)images).PropertyChanged += (_, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(images.Count):
                    Messenger.Send(new ImageCountChangedMessage(images.Count));
                    break;
            }
        };
    }

    public virtual bool CanAddImage => true;
    public SelectorObservableCollection<CaptureImage> Images { get; }

    void OnSelectedImageChanged(CaptureImage? value)
    {
        if (value != null) lastSelectedOption = new(value);
        Messenger.Send(new SelectedImageChangedMessage(value));
    }

    private ImageOption lastSelectedOption = new();

    private void ApplyLastOption(CaptureImage image)
    {
        if (Images.SelectedItem is { } selectedImage)
        {
            image.ImageRatioSize.WidthPercentage = selectedImage.ImageRatioSize.WidthPercentage;
            image.ImageRatioSize.HeightPercentage = selectedImage.ImageRatioSize.HeightPercentage;
            image.ImageKind = selectedImage.ImageKind;
            image.IsSideCutMode = selectedImage.IsSideCutMode;
        }
        else
        {
            image.ImageRatioSize.WidthPercentage = lastSelectedOption.WidthPercentage;
            image.ImageRatioSize.HeightPercentage = lastSelectedOption.HeightPercentage;
            image.ImageKind = lastSelectedOption.ImageKind;
            image.IsSideCutMode = lastSelectedOption.IsSideCutMode;
        }
    }

    public void AddImage(CaptureImage image)
    {
        if (!CanAddImage) return;
        ApplyLastOption(image);
        Images.Add(image);
    }
    public void InsertImage(int index, CaptureImage image)
    {
        if (!CanAddImage) return;
        ApplyLastOption(image);
        Images.Insert(index, image);
    }

    private record ImageOption(
        double WidthPercentage = 100.0,
        double HeightPercentage = 100.0,
        ImageKind ImageKind = ImageKind.Jpg,
        bool IsSideCutMode = false)
    {
        public ImageOption(CaptureImage image) : this(
            image.ImageRatioSize.WidthPercentage,
            image.ImageRatioSize.HeightPercentage,
            image.ImageKind,
            image.IsSideCutMode)
        { }
    }
    void IRecipient<ImageClearRequestMessage>.Receive(ImageClearRequestMessage message)
    {
        Images.Clear();
    }
}
