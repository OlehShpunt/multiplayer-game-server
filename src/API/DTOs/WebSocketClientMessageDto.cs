namespace API;

public struct WebSocketClientMessageDto
{
    public ActionType Action { get; set; }
    public string? Data { get; set; } // NOTE: Can be null, because some actions may not require additional data

    public override string ToString()
    {
        return $"WebSocketDto{{ Action={Action}, Data={Data} }}";
    }

    public readonly bool IsValid()
    {
        return true;
    }

    public enum ActionType
    {
        AddNewPlayerToLobby,
        RemovePlayerFromLobby,
        MovePlayer,
        TeleportPlayer,
    }
}
