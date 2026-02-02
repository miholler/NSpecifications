using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System;

[ExcludeFromCodeCoverage]
internal static class ExceptionPolyfills
{
    extension(ArgumentNullException)
    {
        public static void ThrowIfNull([NotNull]object? argument, [CallerArgumentExpression(nameof(argument))]string? paramName = null)
        {
            if (argument is null)
                throw new ArgumentNullException(paramName);
        }
    }
}
