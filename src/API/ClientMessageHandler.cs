using System.Collections.Concurrent;
using System.Net.WebSockets;
using Application;
using Domain;

namespace API;

public static class ClientMessageHandler
{
    public static async Task HandleNewMessage(
        string clientId,
        WebSocketClientMessageDto dto,
        ConcurrentDictionary<string, WebSocket> connectedClients,
        GameStateManager gameStateManager
    )
    {
        Console.WriteLine("Received message: " + dto.ToString() + " from client " + clientId);
    }
}
