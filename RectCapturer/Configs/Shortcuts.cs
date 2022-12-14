using Kzrnm.Wpf.Input;
using static System.Windows.Input.Key;
using static System.Windows.Input.ModifierKeys;

namespace Kzrnm.RectCapturer.Configs;


/// <summary>
/// 基本の設定
/// </summary>
/// <param name="Post">投稿ボタン</param>
/// <param name="CaptureScreenshot">スクリーンショットを撮る</param>
public record Shortcuts(
    ShortcutKey? Post = null,
    ShortcutKey? CaptureScreenshot = null
)
{
    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='CaptureScreenshot']"/>
    /// </summary>
    public ShortcutKey? CaptureScreenshot { get; init; } = CaptureScreenshot ?? new(Control, B);
    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='Post']"/>
    /// </summary>
    public ShortcutKey? Post { get; init; } = Post ?? new(Control, Enter);


    public Shortcuts() : this(CaptureScreenshot: default) { }
}