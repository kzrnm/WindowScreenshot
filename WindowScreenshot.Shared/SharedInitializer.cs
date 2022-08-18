using Microsoft.Extensions.DependencyInjection;
using WindowScreenshot.Image;
using WindowScreenshot.Image.Capture;
using WindowScreenshot.Windows;

namespace WindowScreenshot;
using ViewModels;

public static class SharedInitializer
{
    public static IServiceCollection InitializeDefault(IServiceCollection service)
        => service
        .AddSingleton(ObserveWindowProcess.Instanse)
        .AddSingleton<ICaptureImageService, CaptureImageService>()
        .AddSingleton<IClipboardManager, ClipboardManager>()
        .AddSingleton<ImageProvider>()
        
        .AddTransient<WindowCapturerViewModel>()
        .AddTransient<ImageListViewModel>()
        .AddTransient<ImageSettingsViewModel>()
        .AddTransient<ImagePreviewWindowViewModel>();
}
