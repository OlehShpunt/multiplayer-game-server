namespace Domain;

public class InventoryComponent
{
    public List<Item> Items { set; get; }
    public int Capacity { set; get; }
    private const int DEFAULT_CAPACITY = 4;

    public InventoryComponent(List<Item> items, int capacity = DEFAULT_CAPACITY)
    {
        Items = items ?? new List<Item>();
        Capacity = capacity;
    }

    public List<Item> GetItems()
    {
        return Items;
    }

    public int GetCapacity()
    {
        return Capacity;
    }

    public void setItems(List<Item> items)
    {
        Items = items;
    }

    public void SetCapacity(int capacity)
    {
        Capacity = capacity;
    }

    public void AddItem(Item item)
    {
        if (Items.Count < Capacity)
        {
            Items.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }
}
