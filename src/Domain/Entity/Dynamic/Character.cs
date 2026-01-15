namespace Domain;

public class Character : IStateTrackable
{
    public int Id { get; init; }
    public LocationComponent Location { get; init; }
    public InventoryComponent Inventory { get; init; }

    public Character(LocationComponent location, InventoryComponent inventory, int id)
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
