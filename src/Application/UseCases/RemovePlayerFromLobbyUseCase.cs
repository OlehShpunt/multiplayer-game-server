namespace Application;

public class RemovePlayerFromLobbyUseCase : UseCase
{
    public RemovePlayerFromLobbyUseCase(IGameStateManager gameStateManager)
        : base(gameStateManager) { }

    public bool Execute(string playerId)
    {
        return GameStateManager.Players.Remove(playerId);
    }
}
