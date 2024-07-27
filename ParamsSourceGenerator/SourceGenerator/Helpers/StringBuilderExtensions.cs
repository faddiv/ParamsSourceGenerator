using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Foxy.Params.SourceGenerator.Helpers;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendJoin<T>(this StringBuilder builder, string? separator, IEnumerable<T> values)
    {
        separator ??= string.Empty;
        return AppendJoinCore(builder, separator, values);
    }

    private static StringBuilder AppendJoinCore<T>(this StringBuilder builder, string separator, IEnumerable<T> values)
    {
        Debug.Assert(separator is not null);

        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        using (IEnumerator<T> en = values.GetEnumerator())
        {
            if (!en.MoveNext())
            {
                return builder;
            }

            T value = en.Current;
            if (value != null)
            {
                builder.Append(value.ToString());
            }

            while (en.MoveNext())
            {
                builder.Append(separator);
                value = en.Current;
                if (value != null)
                {
                    builder.Append(value.ToString());
                }
            }
        }
        return builder;
    }
}
