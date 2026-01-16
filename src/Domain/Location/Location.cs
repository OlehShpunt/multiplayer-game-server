namespace Domain;

public class Location : IStateTrackable
{
    public string Id { get; init; }
    public List<Storage> Storages { get; init; }
    public Scene Scene { get; init; }

    public Location(List<Storage> storages, Scene scene)
    {
        Id = scene.ToString();
        Storages = storages;
        Scene = scene;
    }
}
