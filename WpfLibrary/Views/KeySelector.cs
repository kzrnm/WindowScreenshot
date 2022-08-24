using Kzrnm.Wpf.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kzrnm.Wpf.Views;

public class KeySelector : Control
{
    static KeySelector()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(KeySelector), new FrameworkPropertyMetadata(typeof(KeySelector)));
    }


    public static readonly DependencyProperty HasCtrlProperty =
        DependencyProperty.Register(
            nameof(HasCtrl),
            typeof(bool),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(true)
        );
    public bool HasCtrl
    {
        get => (bool)GetValue(HasCtrlProperty);
        set => SetValue(HasCtrlProperty, value);
    }
    public static readonly DependencyProperty CheckedCtrlProperty =
        DependencyProperty.Register(
            nameof(CheckedCtrl),
            typeof(bool),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((KeySelector)sender).OnModifierKeysUpdated(ModifierKeys.Control, (bool)e.NewValue)
            )
        );
    public bool CheckedCtrl
    {
        get => (bool)GetValue(CheckedCtrlProperty);
        set => SetValue(CheckedCtrlProperty, value);
    }

    public static readonly DependencyProperty HasShiftProperty =
        DependencyProperty.Register(
            nameof(HasShift),
            typeof(bool),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(true)
        );
    public bool HasShift
    {
        get => (bool)GetValue(HasShiftProperty);
        set => SetValue(HasShiftProperty, value);
    }
    public static readonly DependencyProperty CheckedShiftProperty =
        DependencyProperty.Register(
            nameof(CheckedShift),
            typeof(bool),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((KeySelector)sender).OnModifierKeysUpdated(ModifierKeys.Shift, (bool)e.NewValue)
            )
        );
    public bool CheckedShift
    {
        get => (bool)GetValue(CheckedShiftProperty);
        set => SetValue(CheckedShiftProperty, value);
    }

    public static readonly DependencyProperty HasAltProperty =
        DependencyProperty.Register(
            nameof(HasAlt),
            typeof(bool),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(true)
        );
    public bool HasAlt
    {
        get => (bool)GetValue(HasAltProperty);
        set => SetValue(HasAltProperty, value);
    }
    public static readonly DependencyProperty CheckedAltProperty =
        DependencyProperty.Register(
            nameof(CheckedAlt),
            typeof(bool),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((KeySelector)sender).OnModifierKeysUpdated(ModifierKeys.Alt, (bool)e.NewValue)
            )
        );
    public bool CheckedAlt
    {
        get => (bool)GetValue(CheckedAltProperty);
        set => SetValue(CheckedAltProperty, value);
    }

    public static readonly DependencyProperty HasWindowsKeyProperty =
        DependencyProperty.Register(
            nameof(HasWindowsKey),
            typeof(bool),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(true)
        );
    public bool HasWindowsKey
    {
        get => (bool)GetValue(HasWindowsKeyProperty);
        set => SetValue(HasWindowsKeyProperty, value);
    }
    public static readonly DependencyProperty CheckedWindowsKeyProperty =
        DependencyProperty.Register(
            nameof(CheckedWindowsKey),
            typeof(bool),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((KeySelector)sender).OnModifierKeysUpdated(ModifierKeys.Windows, (bool)e.NewValue)
            )
        );
    public bool CheckedWindowsKey
    {
        get => (bool)GetValue(CheckedWindowsKeyProperty);
        set => SetValue(CheckedWindowsKeyProperty, value);
    }

    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register(
            nameof(Key),
            typeof(Key),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(
                Key.None,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((KeySelector)sender).OnKeyUpdated((Key)e.NewValue)
            )
        );
    public Key Key
    {
        get => (Key)GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    public static readonly DependencyProperty ShortcutKeyProperty =
        DependencyProperty.Register(
            nameof(ShortcutKey),
            typeof(ShortcutKey),
            typeof(KeySelector),
            new FrameworkPropertyMetadata(
                default(ShortcutKey),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((KeySelector)sender).OnShortcutKeyUpdated((ShortcutKey)e.NewValue)
            )
        );
    public ShortcutKey ShortcutKey
    {
        get => (ShortcutKey)GetValue(ShortcutKeyProperty);
        set => SetValue(ShortcutKeyProperty, value);
    }

    private void OnModifierKeysUpdated(ModifierKeys modifier, bool isChecked)
    {
        var shortcutModifier = ShortcutKey.Modifiers;
        if (isChecked)
            shortcutModifier |= modifier;
        else
            shortcutModifier &= ~modifier;

        ShortcutKey = ShortcutKey with
        {
            Modifiers = shortcutModifier
        };
    }
    private void OnKeyUpdated(Key key)
    {
        ShortcutKey = ShortcutKey with
        {
            Key = key,
        };
    }
    private void OnShortcutKeyUpdated(ShortcutKey shortcut)
    {
        var modifiers = shortcut.Modifiers;
        CheckedCtrl = modifiers.HasFlag(ModifierKeys.Control);
        CheckedAlt = modifiers.HasFlag(ModifierKeys.Alt);
        CheckedShift = modifiers.HasFlag(ModifierKeys.Shift);
        CheckedWindowsKey = modifiers.HasFlag(ModifierKeys.Windows);
        Key = shortcut.Key;
    }

    private ComboBox? keyComboBox;
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if(keyComboBox != null)
        {
            keyComboBox.KeyDown -= OnKeyComboBoxKeyDown;
        }

        keyComboBox = GetTemplateChild("PART_Key") as ComboBox;

        if (keyComboBox != null)
        {
            keyComboBox.KeyDown += OnKeyComboBoxKeyDown;
        }
    }

    private void OnKeyComboBoxKeyDown(object sender, KeyEventArgs e)
    {
        Key = e.Key;
        e.Handled = true;
    }
}
