namespace Application;

using System.Collections.Concurrent;
using Domain;

public class StateManager<T> : IStateManager<T>
    where T : IStateTrackable
{
    private readonly ConcurrentDictionary<string, T> _items;

    public StateManager()
    {
        _items = new ConcurrentDictionary<string, T>();
    }

    public bool Add(T item)
    {
        return _items.TryAdd(item.Id, item);
    }

    public bool Remove(string id)
    {
        return _items.TryRemove(id, out T? item);
    }

    public T? Get(string id)
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

    public bool Exists(string id)
    {
        return _items.ContainsKey(id);
    }
}
