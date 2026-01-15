namespace Application;

using Domain;

public class GameStateManager : IGameStateManager
{
    public IStateManager<Player> Players { get; init; }
    public IStateManager<Seller> Sellers { get; init; }
    public IStateManager<Customer> Customers { get; init; }
    public IStateManager<Location> Locations { get; init; }
    public IStateManager<Bakery> Bakeries { get; init; }

    public GameStateManager()
    {
        Players = new StateManager<Player>();
        Sellers = new StateManager<Seller>();
        Customers = new StateManager<Customer>();
        Locations = new StateManager<Location>();
        Bakeries = new StateManager<Bakery>();
    }
}
