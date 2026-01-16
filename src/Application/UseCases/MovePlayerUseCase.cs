using System.Numerics;
using Domain;

namespace Application;

public class MovePlayerUseCase : UseCase
{
    public MovePlayerUseCase(IGameStateManager gameStateManager)
        : base(gameStateManager) { }

    public bool Execute(Params parameters)
    {
        var player = GameStateManager.Players.Get(parameters.PlayerId);

        if (player == null)
        {
            return false;
        }

        // Validate if movement to newPosition is allowed (checking boundaries and player speed)
        if (
            !PlayerPositionValidator.IsValid(
                player.Location.Position,
                parameters.NewPosition,
                player.Location.Scene
            )
        )
        {
            Console.WriteLine(
                $"Movement to position {parameters.NewPosition} in scene {SceneConverter.SceneToString(player.Location.Scene)} is not allowed."
            );
            return false;
        }

        player.Location.Position = parameters.NewPosition;

        Console.WriteLine(
            $"Player {parameters.PlayerId} moved to position {player.Location.Position} in scene {SceneConverter.SceneToString(player.Location.Scene)}."
        );

        return true;
    }

    public struct Params
    {
        public string PlayerId { get; set; }
        public Vector2 NewPosition { get; set; }

        public Params(string playerId, Vector2 newPosition)
        {
            PlayerId = playerId;
            NewPosition = newPosition;
        }
    }
}
