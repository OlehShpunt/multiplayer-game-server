namespace Application;

using System.Collections.Concurrent;
using Domain;

public class StateManager<T> : IStateManager<T>
    where T : IStateTrackable
{
    private readonly ConcurrentDictionary<int, T> _items;

    public StateManager()
    {
        _items = new ConcurrentDictionary<int, T>();
    }

    public bool Add(T item)
    {
        return _items.TryAdd(item.Id, item);
    }

    public bool Remove(int id)
    {
        return _items.TryRemove(id, out T? item);
    }

    public T? Get(int id)
    {
        _items.TryGetValue(id, out T? item);
        return item;
    }

    public bool Update(T updatedItem)
    {
        if (!_items.TryGetValue(updatedItem.Id, out T? existingItem))
        {
            return false;
        }

        return _items.TryUpdate(updatedItem.Id, updatedItem, existingItem);
    }

    public IReadOnlyCollection<T> GetAll()
    {
        return _items.Values.ToList();
    }

    public bool Exists(int id)
    {
        return _items.ContainsKey(id);
    }
}
