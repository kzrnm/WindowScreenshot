using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.WindowScreenshot.Image;
using System.ComponentModel;

namespace Kzrnm.WindowScreenshot.ViewModels;

public partial class ImageSettingsViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
{
    public ImageProvider ImageProvider { get; }
    public ImageSettingsViewModel(ImageProvider imageProvider)
        : this(WeakReferenceMessenger.Default, imageProvider)
    { }
    public ImageSettingsViewModel(IMessenger messenger, ImageProvider imageProvider)
        : base(messenger)
    {
        ImageProvider = imageProvider;
        SelectedImage = imageProvider.Images.SelectedItem;
        IsActive = true;
    }

    [ObservableProperty]
    private CaptureImage? _SelectedImage;

    public bool HasImage => SelectedImage is not null;

    public int Height
    {
        set
        {
            if (SelectedImage != null
                && SelectedImage.ImageRatioSize.Height != value)
            {
                SelectedImage.ImageRatioSize.Height = value;
                OnPropertyChanged(_HeightPropertyChangedArgs);
                OnPropertyChanged(_HeightPercentagePropertyChangedArgs);
            }
        }
        get => SelectedImage?.ImageRatioSize.Height ?? 0;
    }
    public int Width
    {
        set
        {
            if (SelectedImage != null
                && SelectedImage.ImageRatioSize.Width != value)
            {
                SelectedImage.ImageRatioSize.Width = value;
                OnPropertyChanged(_WidthPropertyChangedArgs);
                OnPropertyChanged(_WidthPercentagePropertyChangedArgs);
            }
        }
        get => SelectedImage?.ImageRatioSize.Width ?? 0;
    }

    public double WidthPercentage
    {
        set => UpdatePercentage(value);
        get => SelectedImage?.ImageRatioSize.WidthPercentage ?? 0;
    }
    public double HeightPercentage
    {
        set => UpdatePercentage(value);
        get => SelectedImage?.ImageRatioSize.HeightPercentage ?? 0;
    }

    private void UpdatePercentage(double value)
    {
        if (SelectedImage is { } image)
        {
            var imageRatioSize = image.ImageRatioSize;
            if (imageRatioSize.HeightPercentage != value)
            {
                imageRatioSize.HeightPercentage = value;
                OnPropertyChanged(_HeightPropertyChangedArgs);
                OnPropertyChanged(_HeightPercentagePropertyChangedArgs);
            }
            if (imageRatioSize.WidthPercentage != value)
            {
                imageRatioSize.WidthPercentage = value;
                OnPropertyChanged(_WidthPropertyChangedArgs);
                OnPropertyChanged(_WidthPercentagePropertyChangedArgs);
            }
        }
    }

    public ImageKind ImageKind
    {
        set
        {
            if (SelectedImage != null
                && SelectedImage.ImageKind != value)
            {
                OnPropertyChanged(_ImageKindPropertyChangedArgs);
                SelectedImage.ImageKind = value;
            }
        }
        get => SelectedImage?.ImageKind ?? ImageKind.Jpg;
    }
    public bool IsSideCutMode
    {
        set
        {
            if (SelectedImage != null
                && SelectedImage.IsSideCutMode != value)
            {
                OnPropertyChanged(_IsSideCutModePropertyChangedArgs);
                SelectedImage.IsSideCutMode = value;
            }
        }
        get => SelectedImage?.IsSideCutMode ?? false;
    }
    private void SelectedImageChanged(CaptureImage? newImage)
    {
        var oldImage = SelectedImage;
        SelectedImage = newImage;
        if (oldImage == null)
        {
            if (newImage == null)
                return;

            OnPropertyChanged(_HasImagePropertyChangedArgs);
            OnPropertyChanged(_HeightPropertyChangedArgs);
            OnPropertyChanged(_WidthPropertyChangedArgs);
            OnPropertyChanged(_HeightPercentagePropertyChangedArgs);
            OnPropertyChanged(_WidthPercentagePropertyChangedArgs);
            OnPropertyChanged(_ImageKindPropertyChangedArgs);
            OnPropertyChanged(_IsSideCutModePropertyChangedArgs);
            return;
        }

        if (newImage == null)
        {
            OnPropertyChanged(_HasImagePropertyChangedArgs);
            //OnPropertyChanged(_HeightPropertyChangedArgs);
            //OnPropertyChanged(_WidthPropertyChangedArgs);
            //OnPropertyChanged(_HeightPercentagePropertyChangedArgs);
            //OnPropertyChanged(_WidthPercentagePropertyChangedArgs);
            //OnPropertyChanged(_ImageKindPropertyChangedArgs);
            //OnPropertyChanged(_IsSideCutModePropertyChangedArgs);
            return;
        }

        if (oldImage.ImageRatioSize.Height != newImage.ImageRatioSize.Height)
            OnPropertyChanged(_HeightPropertyChangedArgs);
        if (oldImage.ImageRatioSize.Width != newImage.ImageRatioSize.Width)
            OnPropertyChanged(_WidthPropertyChangedArgs);
        if (oldImage.ImageRatioSize.HeightPercentage != newImage.ImageRatioSize.HeightPercentage)
            OnPropertyChanged(_HeightPercentagePropertyChangedArgs);
        if (oldImage.ImageRatioSize.WidthPercentage != newImage.ImageRatioSize.WidthPercentage)
            OnPropertyChanged(_WidthPercentagePropertyChangedArgs);
        if (oldImage.ImageKind != newImage.ImageKind)
            OnPropertyChanged(_ImageKindPropertyChangedArgs);
        if (oldImage.IsSideCutMode != newImage.IsSideCutMode)
            OnPropertyChanged(_IsSideCutModePropertyChangedArgs);
    }

    void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
    {
        SelectedImageChanged(message.Value);
    }

    private static readonly PropertyChangedEventArgs _HasImagePropertyChangedArgs = new(nameof(HasImage));
    private static readonly PropertyChangedEventArgs _WidthPropertyChangedArgs = new(nameof(Width));
    private static readonly PropertyChangedEventArgs _HeightPropertyChangedArgs = new(nameof(Height));
    private static readonly PropertyChangedEventArgs _WidthPercentagePropertyChangedArgs = new(nameof(WidthPercentage));
    private static readonly PropertyChangedEventArgs _HeightPercentagePropertyChangedArgs = new(nameof(HeightPercentage));
    private static readonly PropertyChangedEventArgs _ImageKindPropertyChangedArgs = new(nameof(ImageKind));
    private static readonly PropertyChangedEventArgs _IsSideCutModePropertyChangedArgs = new(nameof(IsSideCutMode));
}
