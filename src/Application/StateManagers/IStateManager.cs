namespace Application;

using Domain;

public interface IStateManager<T>
    where T : IStateTrackable
{
    bool Add(T item);
    bool Remove(string id);
    T? Get(string id);
    bool Update(T updatedItem);
    IReadOnlyCollection<T> GetAll();
    bool Exists(string id);
}
