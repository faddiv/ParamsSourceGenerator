using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Helpers;

public class CollectionComparer
{
    public static CollectionComparer<TElement[], TElement> GetFor<TElement>(
        TElement[] _)
    {
        return CollectionComparer<TElement[], TElement>.Default;
    }
    public static CollectionComparer<TList, TElement> GetFor<TList, TElement>(
        TList _)
        where TList : ICollection<TElement>
    {
        return CollectionComparer<TList, TElement>.Default;
    }

    public static CollectionComparer<TList, TElement> CreateFor<TList, TElement>(
        TList _,
        IEqualityComparer<TElement> elementComparer)
        where TList : ICollection<TElement>
    {
        return new CollectionComparer<TList, TElement>(elementComparer);
    }
}
