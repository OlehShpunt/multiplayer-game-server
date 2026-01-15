namespace Domain;

public class InventoryComponent
{
    public List<Item> Items { get; init; }
    public int Capacity { get; set; }
    private const int DEFAULT_CAPACITY = 4;

    public InventoryComponent(List<Item> items, int capacity = DEFAULT_CAPACITY)
    {
        Items = items ?? new List<Item>();
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
