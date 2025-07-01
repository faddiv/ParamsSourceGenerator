using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Helpers;

public static class CollectionComparer
{
    public static int GetHashCode<TElement>(TElement[]? list)
    {
        int hashCode = 2011230944;
        if (list is null)
            return hashCode;

        var comparer = EqualityComparer<TElement>.Default;
        foreach (var item in list)
        {
            hashCode = hashCode * -1521134295 + comparer.GetHashCode(item);
        }

        return hashCode;
    }
}
