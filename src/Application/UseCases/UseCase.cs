namespace Application;

public abstract class UseCase
{
    public IGameStateManager GameStateManager { get; init; }

    public UseCase(IGameStateManager gameStateManager)
    {
        GameStateManager = gameStateManager;
    }
}
