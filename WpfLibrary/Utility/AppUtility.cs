using Kzrnm.Wpf.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Kzrnm.Wpf.Utility;
public static class AppUtility
{
    public static void OnUnhandledException_Debug(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is not Exception exception) return;
        Debug.WriteLine($"Unhandled: {exception}");
    }

    public static void OnUnhandledException_Release(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is not Exception exception) return;
        var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException();

        Debug.WriteLine(exception);
        Debug.WriteLine(exception.StackTrace);
        var appDir = Path.GetDirectoryName(assembly.Location) ?? throw new InvalidOperationException();
        var logdir = Path.Combine(appDir, "Log");
        Directory.CreateDirectory(logdir);

        var logFilePath = Path.Combine(logdir, $"{assembly.GetName().Name}-{DateTime.Now:yyyyMMddHHmmss}.log");
        using var fw = new StreamWriter(logFilePath, true, new UTF8Encoding(false));
        using var sr = new StringReader(exception.StackTrace ?? "");

        fw.WriteLine("Message： " + exception.Message);
        while (sr.ReadLine() is string line)
            fw.WriteLine(line);
        fw.Flush();

        if (MessageBox.Show(
            messageBoxText: Resources.UnhandledExceptionMessage,
            caption: "UnhandledException",
            MessageBoxButton.YesNo,
            MessageBoxImage.Stop
        ) is MessageBoxResult.Yes)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = logdir,
            });
        }
        e.Handled = true;
    }
}
