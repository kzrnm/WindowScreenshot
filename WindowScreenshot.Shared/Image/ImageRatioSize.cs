﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;

namespace WindowScreenshot.Image;

public class ImageRatioSize : ObservableObject
{
    #region Constructors
    public ImageRatioSize(BitmapSource image)
        : this(image.PixelWidth, image.PixelHeight) { }
    public ImageRatioSize(int width, int height)
    {
        Width = OrigWidth = width;
        Height = OrigHeight = height;
    }

    public ImageRatioSize(BitmapSource image, double percentage)
        : this(image.PixelWidth, image.PixelHeight, percentage) { }
    public ImageRatioSize(int width, int height, double percentage)
        : this(width, height, percentage, percentage) { }

    public ImageRatioSize(BitmapSource image, double widthPercentage, double heightPercentage)
        : this(image.PixelWidth, image.PixelHeight, widthPercentage, heightPercentage) { }
    public ImageRatioSize(int width, int height, double widthPercentage, double heightPercentage)
    {
        OrigWidth = width;
        OrigHeight = height;

        _widthPercentage = widthPercentage;
        _width = (int)(width * widthPercentage / 100);

        _heightPercentage = heightPercentage;
        _height = (int)(height * heightPercentage / 100);
    }
    #endregion

    #region Properties
    public int OrigHeight { get; }
    public int OrigWidth { get; }

    private int _height;
    public int Height
    {
        set
        {
            if (SetProperty(ref _height, value))
            {
                SetProperty(ref _heightPercentage, (double)_height / OrigHeight * 100, nameof(HeightPercentage));
            }
        }
        get => _height;
    }
    private int _width;
    public int Width
    {
        set
        {
            if (SetProperty(ref _width, value))
            {
                SetProperty(ref _widthPercentage, (double)_width / OrigWidth * 100, nameof(WidthPercentage));
            }
        }
        get => _width;
    }

    private double _widthPercentage;
    public double WidthPercentage
    {
        set
        {
            if (SetProperty(ref _widthPercentage, value))
            {
                SetProperty(ref _width, (int)(OrigWidth * value / 100), nameof(Width));
            }
        }
        get => _widthPercentage;
    }
    private double _heightPercentage;
    public double HeightPercentage
    {
        set
        {
            if (SetProperty(ref _heightPercentage, value))
            {
                SetProperty(ref _height, (int)(OrigHeight * value / 100), nameof(Height));
            }
        }
        get => _heightPercentage;
    }
    #endregion

    public bool IsNotChanged => WidthPercentage == 100.0 && HeightPercentage == 100.0;
}
