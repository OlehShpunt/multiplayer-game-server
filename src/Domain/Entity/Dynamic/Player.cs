namespace Domain;

public class Player : Character, IStateTrackable
{
    public string Name { get; set; }
    public int Balance { get; set; }
    private const int DEFAULT_BALANCE = 15;

    public Player(
        string id,
        LocationComponent location,
        InventoryComponent inventory,
        string name,
        int balance = DEFAULT_BALANCE
    )
        : base(location, inventory, id)
    {
        this.Balance = balance;
        this.Name = name;
    }
}
