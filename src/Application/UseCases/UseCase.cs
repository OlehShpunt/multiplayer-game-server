namespace Application;

public class UseCase
{
    public IGameStateManager GameStateManager { get; init; }

    public UseCase(IGameStateManager gameStateManager)
    {
        GameStateManager = gameStateManager;
    }
}
