namespace Domain;

public class Customer : Character
{
    public string Name { get; set; }

    public Customer(
        string id,
        LocationComponent location,
        InventoryComponent inventory,
        string name = ""
    )
        : base(location, inventory, id)
    {
        this.Name = name;
    }
}
