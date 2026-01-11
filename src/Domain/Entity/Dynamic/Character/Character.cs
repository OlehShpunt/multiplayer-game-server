namespace Domain;

public class Character
{
    public int Id { get; set; }
    public LocationComponent Location { get; set; }
    public InventoryComponent Inventory { get; set; }

    public Character(LocationComponent location, InventoryComponent inventory, int id = -1)
    {
        this.Id = id;
        Location = location;
        Inventory = inventory;
    }

    public LocationComponent GetLocation()
    {
        return Location;
    }

    public InventoryComponent GetInventory()
    {
        return Inventory;
    }

    public void SetLocation(LocationComponent location)
    {
        Location = location;
    }

    public void SetInventory(InventoryComponent inventory)
    {
        Inventory = inventory;
    }
}
