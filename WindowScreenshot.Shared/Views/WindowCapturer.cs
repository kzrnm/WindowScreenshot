﻿using CommunityToolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WindowScreenshot.ViewModels;
using DragDrop = GongSolutions.Wpf.DragDrop.DragDrop;

namespace WindowScreenshot.Views;

public class WindowCapturer : DockPanel
{
    static WindowCapturer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCapturer), new FrameworkPropertyMetadata(typeof(WindowCapturer)));
    }

    public WindowCapturer()
    {
        Children.Add(ImageSettings);
        Children.Add(ImageListView);
        SetDock(ImageSettings, Dock.Right);
        SetDock(ImageListView, Dock.Bottom);

        ViewModel = Ioc.Default.GetRequiredServiceIfIsNotInDesignMode<WindowCapturerViewModel>(this);

        SetBinding(AlwaysImageAreaProperty,
            new Binding(nameof(WindowCapturerViewModel.AlwaysImageArea))
            {
                Source = ViewModel,
                Mode = BindingMode.OneWayToSource,
            });
        ImageSettings.SetBinding(WidthProperty,
            new Binding(nameof(SettingsWidth))
            {
                Source = this,
                Mode = BindingMode.OneWay,
            });
        ImageListView.SetBinding(HeightProperty,
            new Binding(nameof(ListHeight))
            {
                Source = this,
                Mode = BindingMode.OneWay,
            });

        var visibilityBinding = new Binding(nameof(ImageVisibility))
        {
            Source = this,
            Mode = BindingMode.OneWay,
        };
        ImageSettings.SetBinding(VisibilityProperty, visibilityBinding);
        ImageListView.SetBinding(VisibilityProperty, visibilityBinding);

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public WindowCapturerViewModel ViewModel { get; }
    public ImageSettings ImageSettings { get; } = new();
    public ImageListView ImageListView { get; } = new();

    public static readonly DependencyProperty ImageWidthProperty =
        DependencyProperty.Register(
            nameof(ImageWidth),
            typeof(double),
            typeof(WindowCapturer),
            new PropertyMetadata(double.NaN));
    public double ImageWidth
    {
        get => (double)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }
    public static readonly DependencyProperty ListHeightProperty =
        DependencyProperty.Register(
            nameof(ListHeight),
            typeof(double),
            typeof(WindowCapturer));
    public double ListHeight
    {
        get => (double)GetValue(ListHeightProperty);
        set => SetValue(ListHeightProperty, value);
    }

    public static readonly DependencyProperty SettingsWidthProperty =
        DependencyProperty.Register(
            nameof(SettingsWidth),
            typeof(double),
            typeof(WindowCapturer),
            new PropertyMetadata(double.NaN));
    public double SettingsWidth
    {
        get => (double)GetValue(SettingsWidthProperty);
        set => SetValue(SettingsWidthProperty, value);
    }

    public static readonly DependencyProperty AlwaysImageAreaProperty =
        DependencyProperty.Register(
            nameof(AlwaysImageArea),
            typeof(bool),
            typeof(WindowCapturer),
            new PropertyMetadata(true));
    public bool AlwaysImageArea
    {
        get => (bool)GetValue(AlwaysImageAreaProperty);
        set => SetValue(AlwaysImageAreaProperty, value);
    }

    private static readonly DependencyProperty ImageVisibilityProperty =
        DependencyProperty.Register(
            nameof(ImageVisibility),
            typeof(Visibility),
            typeof(WindowCapturer));
    private Visibility ImageVisibility
    {
        get => (Visibility)GetValue(ImageVisibilityProperty);
        set => SetValue(ImageVisibilityProperty, value);
    }

    private ImagePreviewWindow? imagePreviewWindow;
    public static readonly DependencyProperty HasPreviewWindowProperty =
        DependencyProperty.Register(
            nameof(HasPreviewWindow),
            typeof(bool),
            typeof(WindowCapturer),
            new PropertyMetadata(true, (d, e) => ((WindowCapturer)d).OnHasPreviewWindowChanged((bool)e.NewValue)));
    public bool HasPreviewWindow
    {
        get => (bool)GetValue(HasPreviewWindowProperty);
        set => SetValue(HasPreviewWindowProperty, value);
    }
    private void OnHasPreviewWindowChanged(bool newValue)
    {
        if (newValue)
        {
            if (imagePreviewWindow == null)
                MakePreviewWindow(Window.GetWindow(this));
        }
        else if (imagePreviewWindow is { } ipw)
        {
            ipw.Owner = null;
            ipw.Close();
        }
    }
    private void MakePreviewWindow(Window window)
    {
        imagePreviewWindow = new()
        {
            Owner = window,
            Top = window.Top + window.Height / 2,
            Left = window.Left + window.Width / 2
        };
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ViewModel.ImageVisibility):
                OnImageVisibilityChanged(ViewModel.ImageVisibility);
                break;
        }
    }

    private void OnImageVisibilityChanged(Visibility visibility)
    {
        if (AlwaysImageArea) return;
        if (Window.GetWindow(this) is not { } window) return;

        if (visibility is Visibility.Collapsed)
        {
            var width = ImageSettings.ActualWidth;
            var height = ImageListView.ActualHeight;
            ImageVisibility = visibility;
            window.Width -= width;
            window.Height -= height;
        }
        else
        {
            ImageVisibility = visibility;
            window.Width += ImageSettings.ActualWidth;
            window.Height += ImageListView.ActualHeight;
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is not { } window) return;
        if (HasPreviewWindow) MakePreviewWindow(window);
        window.Closing += (_, _) => ViewModel.OnWindowClosing();

        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        ImageVisibility = ViewModel.ImageVisibility;

        DragDrop.SetIsDropTarget(this, true);
        DragDrop.SetDropEventType(this, GongSolutions.Wpf.DragDrop.EventType.Bubbled);
        DragDrop.SetDropHandler(this, ViewModel.DropHandler);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
    }

}
