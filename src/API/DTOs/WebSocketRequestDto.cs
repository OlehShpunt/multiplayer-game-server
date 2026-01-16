namespace API;

public struct WebSocketDto
{
    public WebSocketMessage? Message { get; set; }

    public override string ToString()
    {
        return $"WebSocketDto{{ Message={{ Action={Message?.Action}, Data={Message?.Data} }} }}";
    }

    // TODO: add thorough automated unit testing
    public readonly bool IsValid()
    {
        return Message.HasValue && Message.Value.IsValid();
    }
}

public struct WebSocketMessage
{
    public string? Action { get; set; }
    public string? Data { get; set; }

    public readonly bool IsValid()
    {
        // TODO: Action must be one of predefined actions
        return !string.IsNullOrEmpty(Action) && !string.IsNullOrEmpty(Data);
    }
}
