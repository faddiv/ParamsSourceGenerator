using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Helpers;

public class CollectionComparer
{
    public static int GetHashCode<TElement>(IReadOnlyCollection<TElement> list)
    {
        return CollectionComparer<IReadOnlyCollection<TElement>, TElement>.Default.GetHashCode(list);
    }

    public static bool Equals<TElement>(IReadOnlyCollection<TElement> x, IReadOnlyCollection<TElement> y)
    {
        return CollectionComparer<IReadOnlyCollection<TElement>, TElement>.Default.Equals(x, y);
    }
}
