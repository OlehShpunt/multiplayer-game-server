namespace Domain;

public class Storage
{
    public int Id { get; set; }
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
