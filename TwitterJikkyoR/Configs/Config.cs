using Kzrnm.Wpf;
using Kzrnm.Wpf.Font;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using System.Windows;

namespace Kzrnm.TwitterJikkyo.Configs;


/// <summary>
/// 基本の設定
/// </summary>
/// <param name="WindowPosition">MainWindow の位置</param>
/// <param name="Topmost">MainWindow を常に最前面に表示するか</param>
/// <param name="Font">フォント</param>
/// <param name="SaveDstDirectories">保存先のデフォルトディレクトリ</param>
/// <param name="SaveFileNames">保存するファイル名</param>
public record Config(
    WindowPosition? WindowPosition = null,
    bool Topmost = false,
    long DefaultAccountId = 0,
    long DefaultImageAccountId = 0,
    Font? Font = null,
    ImmutableArray<TwitterAccount> Accounts = default
)
{
    /// <summary>
    /// <inheritdoc cref="Config" path="/param[@name='WindowPosition']"/>
    /// </summary>
    [JsonPropertyOrder(int.MinValue)]
    public WindowPosition WindowPosition { get; init; } = WindowPosition ?? new(double.NaN, double.NaN, double.NaN, double.NaN);
    /// <summary>
    /// <inheritdoc cref="Config" path="/param[@name='Font']"/>
    /// </summary>
    [JsonPropertyOrder(int.MinValue)]
    public Font Font { get; init; } = Font ?? new();

    [JsonPropertyOrder(100)]
    public ImmutableArray<TwitterAccount> Accounts { get; init; } = Accounts.GetOrEmpty();

    public Config() : this(WindowPosition: null) { }
}

[JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
public record WindowPosition(double Top, double Left, double Height, double Width)
{
    public void ApplyTo(Window window)
    {
        if (!double.IsNaN(Top)) window.Top = Top;
        if (!double.IsNaN(Left)) window.Left = Left;
        if (!double.IsNaN(Height)) window.Height = Height;
        if (!double.IsNaN(Width)) window.Width = Width;
    }

    public static WindowPosition Load(Window window) => new(window.Top, window.Left, window.Height, window.Width);
}
