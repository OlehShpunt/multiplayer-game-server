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
    public bool Execute(Params parameters)
    {
        return GameStateManager.Players.Add( // Returns false if player with the same ID already exists
            new Player(
                id: parameters.Id,
                location: new LocationComponent(new Vector2(0, 0), Scene.Town),
                inventory: new InventoryComponent(new List<Item>()),
                name: parameters.Name
            )
        );
    }

    public struct Params
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public Params(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
