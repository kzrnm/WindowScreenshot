using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Media.Imaging;

namespace WindowScreenshot.Image;

public partial class ImageProvider : ObservableRecipient, IRecipient<ImageWindowClosingMessage>
{
    public ImageProvider(ICaptureImageService captureImageService) : this(WeakReferenceMessenger.Default, captureImageService, new SelectorObservableCollection<CaptureImage>()) { }
    public ImageProvider(IMessenger messenger, ICaptureImageService captureImageService) : this(messenger, captureImageService, new SelectorObservableCollection<CaptureImage>()) { }
    protected ImageProvider(IMessenger messenger, ICaptureImageService captureImageService, SelectorObservableCollection<CaptureImage> images)
        : base(messenger)
    {
        this.captureImageService = captureImageService;
        Images = images;
        images.SelectedChanged += (_, e) =>
        {
            SelectedImageIndex = e.NewIndex;
            SelectedImage = e.NewItem;
        };
        Messenger.Register(this);
    }

    void IRecipient<ImageWindowClosingMessage>.Receive(ImageWindowClosingMessage message)
    {
        Images.Clear();
    }

    private readonly ICaptureImageService captureImageService;
    public SelectorObservableCollection<CaptureImage> Images { get; }

    [ObservableProperty]
    private int _SelectedImageIndex = -1;
    partial void OnSelectedImageIndexChanged(int value)
    {
        Images.SelectedIndex = value;
    }

    [ObservableProperty]
    private CaptureImage? _SelectedImage;
    partial void OnSelectedImageChanged(CaptureImage? value)
    {
        if (value != null) lastSelectedOption = new(value);
        Messenger.Send(new SelectedImageChangedMessage(value));
    }

    private ImageOption lastSelectedOption = new();

    private void ApplyLastOption(CaptureImage image)
    {
        if (SelectedImage is { } selectedImage)
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

    public virtual bool CanAddImage => true;

    public void AddImage(BitmapSource bmp)
    {
        if (!CanAddImage) return;
        bmp.Freeze();
        var image = new CaptureImage(bmp);
        ApplyLastOption(image);
        Images.Add(image);
    }
    public bool TryAddImageFromFile(string filePath)
    {
        if (!CanAddImage) return false;
        var image = captureImageService.GetImageFromFile(filePath);
        if (image == null) return false;
        ApplyLastOption(image);
        Images.Add(image);
        return true;
    }
    public void InsertImage(int index, BitmapSource bmp)
    {
        if (!CanAddImage) return;
        bmp.Freeze();
        var image = new CaptureImage(bmp);
        ApplyLastOption(image);
        Images.Insert(index, image);
    }
    public bool TryInsertImageFromFile(int index, string filePath)
    {
        if (!CanAddImage) return false;
        var image = captureImageService.GetImageFromFile(filePath);
        if (image == null) return false;
        ApplyLastOption(image);
        Images.Insert(index, image);
        return true;
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
}
