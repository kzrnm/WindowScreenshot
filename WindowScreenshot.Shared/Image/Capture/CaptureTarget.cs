using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace WindowScreenshot.Image.Capture;

public partial class CaptureTarget : ObservableObject, ICloneable
{
#pragma warning disable IDE0060 // 未使用のパラメーターを削除します
    partial void OnWindowNameChanged(string value)
        => OnPropertyChanged(nameof(DisplayText));
    partial void OnProcessNameChanged(string value)
        => OnPropertyChanged(nameof(DisplayText));
#pragma warning restore IDE0060 // 未使用のパラメーターを削除します

    [ObservableProperty]
    private string _ProcessName = "";

    [ObservableProperty]
    private string _WindowName = "";

    [ObservableProperty]
    private bool _UseOnlyTargetWindow = true;

    [ObservableProperty]
    private CaptureRegion _Region = new();

    [JsonIgnore]
    public string DisplayText
    {
        get
        {
            if (string.IsNullOrEmpty(this.WindowName))
                if (string.IsNullOrEmpty(this.ProcessName)) return "{none}";
                else return this.ProcessName;
            else return $"{ProcessName}[{WindowName}]";
        }
    }

    public bool IsFitFor(WindowProcessHandle window)
    {
        if (!window.IsActive)
            return false;

        if (ProcessName != null
            && (window.ProcessName.IndexOf(this.ProcessName, StringComparison.OrdinalIgnoreCase) < 0))
        {
            return false;
        }
        if (WindowName != null
            && (window.GetCurrentWindowName().IndexOf(WindowName, StringComparison.OrdinalIgnoreCase) < 0))
        {
            return false;
        }
        return true;
    }

    public BitmapSource? CaptureFrom(WindowProcessHandle[] windowProcessHandles)
    {
        foreach (var window in windowProcessHandles)
        {
            if (IsFitFor(window))
            {
                return window.GetClientBitmap(this.Region, this.UseOnlyTargetWindow);
            }
        }
        return default;
    }

    object ICloneable.Clone() => Clone();
    public CaptureTarget Clone()
    {
        var obj = (CaptureTarget)MemberwiseClone();
        obj.Region = Region.Clone();
        return obj;
    }
}