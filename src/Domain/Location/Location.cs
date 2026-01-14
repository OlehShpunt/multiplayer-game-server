namespace Domain;

public class Location : IStateTrackable
{
    public int Id { get; set; }
    public List<Storage> Storages { get; init; }
    public Scene Scene { get; set; }

    public Location(int id, List<Storage> storages, Scene scene)
    {
        Id = id;
        Storages = storages;
        Scene = scene;
    }
}
