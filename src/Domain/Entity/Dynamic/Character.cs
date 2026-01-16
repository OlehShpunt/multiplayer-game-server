namespace Domain;

public class Character : IStateTrackable
{
    public string Id { get; init; }
    public LocationComponent Location { get; init; }
    public InventoryComponent Inventory { get; init; }

    public Character(LocationComponent location, InventoryComponent inventory, string id)
    {
        this.Id = id;
        Location = location;
        Inventory = inventory;
    }
}
