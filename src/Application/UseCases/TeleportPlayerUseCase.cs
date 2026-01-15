using System.Numerics;
using Domain;

namespace Application;

public class TeleportPlayerUseCase : UseCase
{
    public TeleportPlayerUseCase(IGameStateManager gameStateManager)
        : base(gameStateManager) { }

    public bool Execute(int playerId, Scene toScene)
    {
        var player = GameStateManager.Players.Get(playerId);

        if (player == null)
        {
            Console.WriteLine($"Player with ID {playerId} not found.");
            return false;
        }

        // Copy the values
        var fromPosition = player.Location.Position; // Position is of type Vector2, which is a struct (structs are copied by value, not reference)
        var fromScene = player.Location.Scene;

        // Validate if teleportation is allowed at fromPosition, i.e., whether the player is at teleport's position
        if (!TeleportPositionValidator.IsValid(fromPosition, fromScene))
        {
            Console.WriteLine(
                $"Teleportation from position {fromPosition} to scene {toScene} is not allowed."
            );
            return false;
        }

        player.Location.Position = TeleportPositionResolver.Resolve(player.Location.Scene, toScene);
        player.Location.Scene = toScene;

        Console.WriteLine(
            $"Player {playerId} teleported from scene "
                + $"{SceneConverter.SceneToString(fromScene)} to scene {SceneConverter.SceneToString(player.Location.Scene)} "
                + $"with previous position {fromPosition} and new position {player.Location.Position}."
        );

        return true;
    }
}
