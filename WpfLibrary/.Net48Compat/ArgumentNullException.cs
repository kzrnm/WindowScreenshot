using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#if NETFRAMEWORK
namespace Kzrnm.Wpf;
public class ArgumentNullException : System.ArgumentNullException
{
    public ArgumentNullException(string paramName) : base(paramName)
    {
    }

    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument == null)
            Throw(paramName);
    }

    [DoesNotReturn]
    static void Throw(string? paramName) =>
        throw new System.ArgumentNullException(paramName);
}
#endif