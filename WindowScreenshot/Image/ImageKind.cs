using System;
using System.ComponentModel;

namespace Kzrnm.WindowScreenshot.Image;

public enum ImageKind
{
    [Description("jpeg")]
    Jpg,
    [Description("png")]
    Png,
}
public static class ImageKindExtension
{
    /// <summary>
    /// 拡張子を返します
    /// </summary>
    public static string GetExtension(this ImageKind kind) => kind switch
    {
        ImageKind.Jpg => "jpg",
        ImageKind.Png => "png",
        _ => throw new NotSupportedException(),
    };
}
