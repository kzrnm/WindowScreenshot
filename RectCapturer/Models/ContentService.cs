using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Kzrnm.RectCapturer.Properties;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kzrnm.RectCapturer.Models;

public partial class ContentService : ObservableObject
{
    public ContentService(ImageProvider imageProvider)
    {
        ImageProvider = imageProvider;
        UpdateCanPost();
        ((INotifyPropertyChanged)ImageProvider.Images).PropertyChanged += OnImageProviderPropertyChanged;
    }

    private void OnImageProviderPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ImageProvider.Images.Count):
                UpdateCanPost();
                return;
        }
    }

    public event EventHandler<bool>? CanPostChanged;
    public ImageProvider ImageProvider { get; }

    private bool _CanPost;
    public bool CanPost
    {
        private set
        {
            if (SetProperty(ref _CanPost, value))
            {
                CanPostChanged?.Invoke(this, value);
            }
        }
        get => _CanPost;
    }

    [ObservableProperty]
    private string _DestinationFolder = "";

    [ObservableProperty]
    private string _SaveFileName = "";
    private void UpdateCanPost()
    {
        CanPost = ImageProvider.Images.Count > 0;
    }

    public async Task PostContentAsync()
    {
        if (!CanPost) return;
        var dst = new DirectoryInfo(DestinationFolder);
        var fileName = SaveFileName;

        if (!dst.Exists)
        {
            var response = WeakReferenceMessenger.Default.Send(new DialogMessage
            {
                Text = Resources.DirectoryDoesNotExist,
                MessageBoxButton = MessageBoxButton.YesNo,
                MessageBoxImage = MessageBoxImage.Warning,
            }).Response;

            if (response == MessageBoxResult.Yes)
            {
                try
                {
                    dst.Create();
                }
                catch (IOException)
                {
                    WeakReferenceMessenger.Default.Send(new DialogMessage
                    {
                        Text = Resources.FailedToCreateDirectory,
                        MessageBoxButton = MessageBoxButton.OK,
                        MessageBoxImage = MessageBoxImage.Error,
                    });
                    return;
                }
            }
        }

        static int? ParseFileNameNumber(FileInfo fileInfo, ReadOnlySpan<char> commonPrefix)
        {
            var fileName = fileInfo.Name;
            if (!fileName.AsSpan().StartsWith(commonPrefix, StringComparison.OrdinalIgnoreCase))
                return null;
            fileName = fileName[commonPrefix.Length..^fileInfo.Extension.Length];
            if (fileName.Length > 0 && fileName[0] != '_')
                return null;
            return int.TryParse(fileName[1..], out var res) ? res : null;
        }

        var indecis = dst.EnumerateFiles($"{fileName}_*")
            .Select(f => ParseFileNameNumber(f, fileName.AsSpan()))
            .OfType<int>()
            .ToArray();
        var num = indecis.DefaultIfEmpty(0).Max() + 1;

        var tasks = new List<Task>(ImageProvider.Images.Count);
        foreach (var image in ImageProvider.Images)
        {
            var outputPath = Path.Combine(dst.FullName, $"{fileName}_{num++:000}.{image.ImageKind.GetExtension()}");
            tasks.Add(WriteImageAsync(image, outputPath));
        }
        await Task.WhenAll(tasks).ConfigureAwait(false);
        ImageProvider.Images.Clear();

        static async Task WriteImageAsync(CaptureImage image, string outputPath)
        {
            using var stream = image.ToStream();
            using var outputStream = new FileStream(outputPath, FileMode.CreateNew);
            await stream.CopyToAsync(outputStream).ConfigureAwait(false);
        }
    }
}
