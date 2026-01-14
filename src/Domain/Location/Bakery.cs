namespace Domain;

public class Bakery : Location
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Bakery(int id, string name, List<Storage> storages, Scene scene)
        : base(storages, scene)
    {
        Id = id;
        Name = name;
    }
}
