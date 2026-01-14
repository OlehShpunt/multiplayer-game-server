namespace Domain;

public class Player : Character, IStateTrackable
{
    public string Name { get; set; }
    public int Balance { set; get; }
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

    public string GetName()
    {
        return Name;
    }

    public int GetBalance()
    {
        return Balance;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetBalance(int balance)
    {
        Balance = balance;
    }
}
