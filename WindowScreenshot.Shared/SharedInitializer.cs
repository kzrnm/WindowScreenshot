using Microsoft.Extensions.DependencyInjection;
using WindowScreenshot.Image.Capture;
using WindowScreenshot.Windows;

namespace WindowScreenshot;
public static class SharedInitializer
{
    public static IServiceCollection InitializeDefault(IServiceCollection service)
        => service
        .AddSingleton(ObserveWindowProcess.Instanse)
        .AddSingleton<IClipboardManager, ClipboardManager>();

}
