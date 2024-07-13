using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Helpers;

public class CollectionComparer
{
    public static CollectionComparer<TElement[], TElement> GetFor<TElement>(
        TElement[] _)
    {
        return CollectionComparer<TElement[], TElement>.Default;
    }

    public static CollectionComparer<List<TElement>, TElement> GetFor<TElement>(
        List<TElement> _)
    {
        return CollectionComparer<List<TElement>, TElement>.Default;
    }

    public static CollectionComparer<IReadOnlyList<TElement>, TElement> GetFor<TElement>(
        IReadOnlyList<TElement> list)
    {
        return GetFor<IReadOnlyList<TElement>, TElement>(list);
    }

    public static CollectionComparer<TList, TElement> GetFor<TList, TElement>(
        TList _)
        where TList : IReadOnlyCollection<TElement>
    {
        return CollectionComparer<TList, TElement>.Default;
    }

    public static CollectionComparer<TList, TElement> CreateFor<TList, TElement>(
        TList _,
        IEqualityComparer<TElement> elementComparer)
        where TList : IReadOnlyCollection<TElement>
    {
        return new CollectionComparer<TList, TElement>(elementComparer);
    }
}
