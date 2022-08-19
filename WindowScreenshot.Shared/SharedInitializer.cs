using Microsoft.Extensions.DependencyInjection;
using Kzrnm.WindowScreenshot.Image;
using Kzrnm.WindowScreenshot.Image.Capture;
using Kzrnm.WindowScreenshot.Windows;

namespace Kzrnm.WindowScreenshot;
using ViewModels;

public static class SharedInitializer
{
    public static IServiceCollection InitializeDefault(
        this IServiceCollection service,
        ObserveWindowProcess? observeWindowProcess = null,
        ICaptureImageService? captureImageService = null,
        IClipboardManager? clipboardManager = null,
        ImageProvider? imageProvider = null
        )
    {
        service.AddSingleton(observeWindowProcess ?? ObserveWindowProcess.Instanse);

        if (captureImageService is null)
            service.AddSingleton<ICaptureImageService, CaptureImageService>();
        else
            service.AddSingleton(captureImageService);

        if (clipboardManager is null)
            service.AddSingleton<IClipboardManager, ClipboardManager>();
        else
            service.AddSingleton(clipboardManager);

        if (imageProvider is null)
            service.AddSingleton<ImageProvider>();
        else
            service.AddSingleton(imageProvider);

        return service
            .AddTransient<WindowCapturerViewModel>()
            .AddTransient<ImageListViewModel>()
            .AddTransient<ImageSettingsViewModel>()
            .AddTransient<ImagePreviewWindowViewModel>();
    }
}
