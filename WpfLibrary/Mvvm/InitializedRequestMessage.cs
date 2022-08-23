using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Diagnostics.CodeAnalysis;

namespace Kzrnm.Wpf.Mvvm;
public class InitializedRequestMessage<T> : RequestMessage<T?>
{
    public InitializedRequestMessage([DisallowNull] T initialValue)
    {
        InitialValue = initialValue;
    }
    [MemberNotNull]
    public T InitialValue { get; }
}
