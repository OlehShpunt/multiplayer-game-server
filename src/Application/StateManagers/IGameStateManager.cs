namespace Application;

using Domain;

public interface IGameStateManager
{
    IStateManager<Player> Players { get; }
    IStateManager<Seller> Sellers { get; }
    IStateManager<Customer> Customers { get; }
    IStateManager<Location> Locations { get; }
    IStateManager<Bakery> Bakeries { get; }
}
