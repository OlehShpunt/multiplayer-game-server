namespace Domain;

public class Storage
{
    // NOTE: Cannot use LocationComponent, because a Storage instance is injected into an object of type Location, which already specifies the scene where this Storage is located.
    public int Id { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public InventoryComponent Inventory { get; set; }

    public Storage(InventoryComponent inventory, int id = -1)
    {
        Id = id;
        Inventory = inventory;
    }

    public int GetId()
    {
        return Id;
    }

    public void SetId(int id)
    {
        Id = id;
    }

    public InventoryComponent GetInventory()
    {
        return Inventory;
    }

    public void SetInventory(InventoryComponent inventory)
    {
        Inventory = inventory;
    }
}
