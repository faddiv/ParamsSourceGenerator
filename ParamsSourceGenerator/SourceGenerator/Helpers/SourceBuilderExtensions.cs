using System.Runtime.CompilerServices;

namespace Foxy.Params.SourceGenerator.Helpers;

public static class SourceBuilderExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "Used by InterpolatedStringHandler")]
    internal static void AppendLine(
        this SourceBuilder builder,
        [InterpolatedStringHandlerArgument("builder")]in SourceBuilder.InterpolatedStringHandler input)
    {
        input.FinishLine();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "Used by InterpolatedStringHandler")]
    internal static void AddFormatted(
        this SourceBuilder.SourceLine builder,
        [InterpolatedStringHandlerArgument("builder")] in SourceBuilder.SourceLine.InterpolatedStringHandler input)
    {
    }
}
