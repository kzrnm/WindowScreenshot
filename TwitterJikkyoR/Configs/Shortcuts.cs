using Kzrnm.Wpf.Input;
using static System.Windows.Input.Key;
using static System.Windows.Input.ModifierKeys;

namespace Kzrnm.TwitterJikkyo.Configs;


/// <summary>
/// 基本の設定
/// </summary>
/// <param name="Post">投稿ボタン</param>
/// <param name="CaptureScreenshot">スクリーンショットを撮る</param>
/// <param name="ToggleHashtag">ハッシュタグ ON/OFF</param>
/// <param name="InputInReplyTo">リプライ先を入力</param>
public record Shortcuts(
    ShortcutKey? Post = null,
    ShortcutKey? CaptureScreenshot = null,
    ShortcutKey? ToggleHashtag = null,
    ShortcutKey? InputInReplyTo = null,
    ShortcutKey? ActivatePreviousAccount = null,
    ShortcutKey? ActivateNextAccount = null,
    ShortcutKey? ActivatePreviousImageAccount = null,
    ShortcutKey? ActivateNextImageAccount = null
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
    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ToggleHashtag']"/>
    /// </summary>
    public ShortcutKey? ToggleHashtag { get; init; } = ToggleHashtag ?? new(Control, T);
    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='InputInReplyTo']"/>
    /// </summary>
    public ShortcutKey? InputInReplyTo { get; init; } = InputInReplyTo ?? new(Control, R);    

    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ActivatePreviousAccount']"/>
    /// </summary>
    public ShortcutKey? ActivatePreviousAccount { get; init; } = ActivatePreviousAccount ?? new(Control | Shift, U);

    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ActivateNextAccount']"/>
    /// </summary>
    public ShortcutKey? ActivateNextAccount { get; init; } = ActivateNextAccount ?? new(Control, U);

    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ActivatePreviousImageAccount']"/>
    /// </summary>
    public ShortcutKey? ActivatePreviousImageAccount { get; init; } = ActivatePreviousImageAccount ?? new(Control | Shift, I);

    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ActivateNextImageAccount']"/>
    /// </summary>
    public ShortcutKey? ActivateNextImageAccount { get; init; } = ActivateNextImageAccount ?? new(Control, I);

    public Shortcuts() : this(CaptureScreenshot: default) { }
}