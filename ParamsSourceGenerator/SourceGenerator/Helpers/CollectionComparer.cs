using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Helpers;

public class CollectionComparer
{
    public static CollectionComparer<IReadOnlyCollection<TElement>, TElement> GetFor<TElement>(IReadOnlyCollection<TElement> _)
    {
        return CollectionComparer<IReadOnlyCollection<TElement>, TElement>.Default;
    }

    public static int GetHashCode<TElement>(IReadOnlyCollection<TElement> list)
    {
        return GetFor(list).GetHashCode(list);
    }

    public static bool Equals<TElement>(IReadOnlyCollection<TElement> x, IReadOnlyCollection<TElement> y)
    {
        return GetFor(x).Equals(x, y);
    }
}
