namespace Domain;

public class Bakery : Location
{
    public string Name { get; set; }

    public Bakery(string name, List<Storage> storages, Scene scene)
        : base(storages, scene)
    {
        Name = name;
    }
}
