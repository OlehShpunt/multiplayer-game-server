namespace Application;

using Domain;

public class GameStateManager
{
    public StateManager<Player> Players { get; init; }
    public StateManager<Seller> Sellers { get; init; }
    public StateManager<Customer> Customers { get; init; }
    public StateManager<Location> Locations { get; init; }
    public StateManager<Bakery> Bakeries { get; init; }

    public GameStateManager()
    {
        Players = new StateManager<Player>();
        Sellers = new StateManager<Seller>();
        Customers = new StateManager<Customer>();
        Locations = new StateManager<Location>();
        Bakeries = new StateManager<Bakery>();
    }
}
