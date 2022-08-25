using Kzrnm.Wpf.Mvvm;

namespace Kzrnm.Wpf.Models;

public class TextInputDialogMessage : InitializedRequestMessage<string>
{
    public TextInputDialogMessage(string initialValue) : base(initialValue)
    {
    }

    public string? Title { get; init; }
    public string? Message { get; init; }
    public double? Width { get; init; }
    public double? Height { get; init; }
    public double? MinWidth { get; init; }
    public double? MinHeight { get; init; }
}
