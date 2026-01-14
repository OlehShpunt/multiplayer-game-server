namespace Domain;

public class Bakery : Location
{
    public string Name { get; set; }

    public Bakery(int id, string name, List<Storage> storages, Scene scene)
        : base(id, storages, scene)
    {
        Name = name;
    }
}
