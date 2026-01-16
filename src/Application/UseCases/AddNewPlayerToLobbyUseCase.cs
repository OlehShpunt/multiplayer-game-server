using System.Numerics;
using Domain;

namespace Application;

public class AddNewPlayerToLobbyUseCase : UseCase
{
    public AddNewPlayerToLobbyUseCase(IGameStateManager gameStateManager)
        : base(gameStateManager) { }

    /// <summary>
    ///  Adds a new player to the Town scene with the specified ID and name.
    /// </summary>
    /// <returns>True if the player was added successfully; false if the player already exists.</returns>
    public bool Execute(string id, string name)
    {
        return GameStateManager.Players.Add( // Returns false if player with the same ID already exists
            new Player(
                id: id,
                location: new LocationComponent(new Vector2(0, 0), Scene.Town),
                inventory: new InventoryComponent(new List<Item>()),
                name: name
            )
        );
    }
}
