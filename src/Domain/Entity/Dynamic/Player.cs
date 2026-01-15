namespace Domain;

public class Player : Character, IStateTrackable
{
    public string Name { get; set; }
    public int Balance { get; set; }
    private const int DEFAULT_BALANCE = 15;

    public Player(
        int id,
        LocationComponent location,
        InventoryComponent inventory,
        int balance = DEFAULT_BALANCE,
        string name = ""
    )
        : base(location, inventory, id)
    {
        this.Balance = balance;
        this.Name = name;
    }
}
