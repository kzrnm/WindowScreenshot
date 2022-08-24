using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Kzrnm.Wpf.Mvvm;

public interface IEnumWithText
{
    Enum Value { get; }
    string Text { get; }
}
public record EnumWithText(Enum Value, string Text) : IEnumWithText;
public record EnumWithText<T>(T Value, string Text) : IEnumWithText where T : Enum
{
    Enum IEnumWithText.Value => Value;
}
