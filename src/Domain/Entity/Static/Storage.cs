using System.Numerics;

namespace Domain;

public class Storage
{
    // NOTE: Cannot use LocationComponent, because a Storage instance is injected into an object of type Location, which already specifies the scene where this Storage is located.
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public InventoryComponent Inventory { get; set; }

    public Storage(int id, InventoryComponent inventory, Vector2 position)
    {
        Id = id;
        Inventory = inventory;
        Position = position;
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
