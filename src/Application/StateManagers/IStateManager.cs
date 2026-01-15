namespace Application;

using Domain;

public interface IStateManager<T>
    where T : IStateTrackable
{
    bool Add(T item);
    bool Remove(int id);
    T? Get(int id);
    bool Update(T updatedItem);
    IReadOnlyCollection<T> GetAll();
    bool Exists(int id);
}
