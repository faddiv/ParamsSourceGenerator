using System.Collections.Concurrent;
using System.Threading;

namespace Foxy.Params.SourceGenerator.Helpers;

internal class SourceBuilderPool
{
    public static SourceBuilderPool Instance { get; } = new();

    private readonly int _maxCapacity = 8 - 1; // -1 to account for _fastItem
    private int _numItems;

    private readonly ConcurrentQueue<SourceBuilder> _items = new();
    private SourceBuilder? _fastItem;

    public SourceBuilder Get()
    {
        var item = _fastItem;
        if (item == null || Interlocked.CompareExchange(ref _fastItem, null, item) != item)
        {
            if (_items.TryDequeue(out item))
            {
                Interlocked.Decrement(ref _numItems);
                return item;
            }

            // no object available, so go get a brand new one
            return new();
        }

        return item;
    }

    public void Return(SourceBuilder obj)
    {
        obj.Clear();

        if (_fastItem == null && Interlocked.CompareExchange(ref _fastItem, obj, null) == null)
            return;

        if (Interlocked.Increment(ref _numItems) <= _maxCapacity)
        {
            _items.Enqueue(obj);
            return;
        }

        // no room, clean up the count and drop the object on the floor
        Interlocked.Decrement(ref _numItems);
    }
}
