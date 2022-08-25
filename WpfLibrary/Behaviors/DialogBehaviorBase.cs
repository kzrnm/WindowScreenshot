using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Kzrnm.Wpf.Behaviors;

public abstract class DialogBehaviorBase<TSelf, TMessage> : Behavior<Control>, IRecipient<TMessage>
    where TSelf : DialogBehaviorBase<TSelf, TMessage>
    where TMessage : class
{
    public static readonly DependencyProperty TokenProperty =
        DependencyProperty.Register(
            nameof(Token),
            typeof(Guid),
            typeof(DialogBehaviorBase<TSelf, TMessage>),
            new PropertyMetadata(Guid.Empty));
    public Guid Token
    {
        get => (Guid)GetValue(TokenProperty);
        set => SetValue(TokenProperty, value);
    }

    protected override void OnAttached()
    {
        if (Token == Guid.Empty)
            WeakReferenceMessenger.Default.Register(this);
        else
            WeakReferenceMessenger.Default.Register(this, Token);
        AssociatedObject.Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        Detach();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Unloaded -= OnUnloaded;
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    protected Window GetWindow() => Window.GetWindow(AssociatedObject);
    public abstract void Receive(TMessage message);
}