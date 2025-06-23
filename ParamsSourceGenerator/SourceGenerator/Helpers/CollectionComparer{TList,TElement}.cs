using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Helpers;

public class CollectionComparer<TList, TElement>(IEqualityComparer<TElement>? elementComparer = null)
    : CollectionComparer,
        IEqualityComparer<TList>
    where TList : IReadOnlyCollection<TElement>
{
    public static readonly CollectionComparer<TList, TElement> Default = new();

    private IEqualityComparer<TElement> ElementComparer { get; } = elementComparer ?? EqualityComparer<TElement>.Default;

    public bool Equals(TList? x, TList? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return false;
        }

        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x.Count != y.Count)
        {
            return false;
        }

        using var e1 = x.GetEnumerator();
        using var e2 = y.GetEnumerator();
        while (e1.MoveNext() && e2.MoveNext())
        {
            if (!ElementComparer.Equals(e1.Current, e2.Current))
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode(TList? obj)
    {
        int hashCode = 2011230944;
        if (obj is not null)
        {
            foreach (var item in obj)
            {
                hashCode = hashCode * -1521134295 + ElementComparer.GetHashCode(item);
            }
        }

        return hashCode;
    }
}
