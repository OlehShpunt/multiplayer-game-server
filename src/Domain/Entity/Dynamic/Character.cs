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
}
