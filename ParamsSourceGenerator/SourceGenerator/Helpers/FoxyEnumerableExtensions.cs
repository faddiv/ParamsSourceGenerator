using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Helpers;

internal static class FoxyEnumerableExtensions
{
    public static IncrementalValuesProvider<TSource> NotNull<TSource>(
        this IncrementalValuesProvider<TSource?> enumerable)
        where TSource : class
    {
        return enumerable.Where(x => x is not null)!;
    }

    public static F[] Convert<T, F>(this ReadOnlySpan<T> values, Func<T, F> func)
    {
        var result = new F[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            var value = func(values[i]);
            result[i] = value;
        }
        return result;
    }
}
