namespace Domain;

public class Location
{
    public List<Storage> Storages { get; init; }
    public Scene Scene { get; set; }

    public Location(List<Storage> storages, Scene scene)
    {
        Storages = storages;
        Scene = scene;
    }
}
