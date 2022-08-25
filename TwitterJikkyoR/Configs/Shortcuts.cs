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
    ShortcutKey? ActivatePreviousUser = null,
    ShortcutKey? ActivateNextUser = null,
    ShortcutKey? ActivatePreviousImageUser = null,
    ShortcutKey? ActivateNextImageUser = null
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
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ActivatePreviousUser']"/>
    /// </summary>
    public ShortcutKey? ActivatePreviousUser { get; init; } = ActivatePreviousUser ?? new(Control | Shift, U);

    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ActivateNextUser']"/>
    /// </summary>
    public ShortcutKey? ActivateNextUser { get; init; } = ActivateNextUser ?? new(Control, U);

    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ActivatePreviousImageUser']"/>
    /// </summary>
    public ShortcutKey? ActivatePreviousImageUser { get; init; } = ActivatePreviousImageUser ?? new(Control | Shift, I);

    /// <summary>
    /// <inheritdoc cref="Shortcuts" path="/param[@name='ActivateNextImageUser']"/>
    /// </summary>
    public ShortcutKey? ActivateNextImageUser { get; init; } = ActivateNextImageUser ?? new(Control, I);

    public Shortcuts() : this(CaptureScreenshot: default) { }
}