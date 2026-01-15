namespace Domain;

public class Seller : Character
{
    public string Name { get; set; }

    public Seller(
        int id,
        LocationComponent location,
        InventoryComponent inventory,
        string name = ""
    )
        : base(location, inventory, id)
    {
        this.Name = name;
    }
}
