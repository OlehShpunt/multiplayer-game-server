namespace Domain;

public class Location : IStateTrackable
{
    public int Id { get; init; }
    public List<Storage> Storages { get; init; }
    public Scene Scene { get; init; }

    public Location(List<Storage> storages, Scene scene)
    {
        Id = (int)scene;
        Storages = storages;
        Scene = scene;
    }
}
