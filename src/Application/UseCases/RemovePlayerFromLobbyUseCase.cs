namespace Application;

public class RemovePlayerFromLobbyUseCase : UseCase
{
    public RemovePlayerFromLobbyUseCase(IGameStateManager gameStateManager)
        : base(gameStateManager) { }

    public bool Execute(Params parameters)
    {
        return GameStateManager.Players.Remove(parameters.PlayerId);
    }

    public struct Params
    {
        public string PlayerId { get; set; }

        public Params(string playerId)
        {
            PlayerId = playerId;
        }
    }
}
