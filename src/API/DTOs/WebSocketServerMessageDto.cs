namespace API;

public struct WebSocketServerMessageDto
{
    public ActionType Action { get; set; }
    public string? Data { get; set; } // NOTE: Can be null, because some actions may not require additional data

    public WebSocketServerMessageDto(ActionType action, string? data)
    {
        Action = action;
        Data = data;
    }

    public override readonly string ToString()
    {
        return $"WebSocketDto{{ Action={Action}, Data={Data} }}";
    }

    public enum ActionType
    {
        AddNewPlayerToLobby,
        RemovePlayerFromLobby,
        MovePlayer,
        TeleportPlayer,
        StartGame,
        EndGame,
    }
}
