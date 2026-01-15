using System.Numerics;
using Domain;

namespace Application;

public class MovePlayerUseCase : UseCase
{
    public MovePlayerUseCase(IGameStateManager gameStateManager)
        : base(gameStateManager) { }

    public bool Execute(int playerId, Vector2 newPosition)
    {
        var player = GameStateManager.Players.Get(playerId);

        if (player == null)
        {
            return false;
        }

        // Validate if movement to newPosition is allowed (checking boundaries and player speed)
        if (
            !PlayerPositionValidator.IsValid(
                player.Location.Position,
                newPosition,
                player.Location.Scene
            )
        )
        {
            Console.WriteLine(
                $"Movement to position {newPosition} in scene {SceneConverter.SceneToString(player.Location.Scene)} is not allowed."
            );
            return false;
        }

        player.Location.Position = newPosition;

        Console.WriteLine(
            $"Player {playerId} moved to position {player.Location.Position} in scene {SceneConverter.SceneToString(player.Location.Scene)}."
        );

        return true;
    }
}
