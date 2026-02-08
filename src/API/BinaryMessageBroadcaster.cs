using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace API;

public static class BinaryMessageBroadcaster
{
    public static async Task BroadcastMessageToAllAsync(
        byte[] message,
        ConcurrentDictionary<string, WebSocket> connectedClients
    )
    {
        await SendMessageAsync(connectedClients.Values, message, connectedClients);
    }

    public static async Task BroadcastMessageToSpecificAsync(
        IEnumerable<string> ids,
        byte[] message,
        ConcurrentDictionary<string, WebSocket> connectedClients
    )
    {
        var clients = new List<WebSocket>();

        foreach (string id in ids)
        {
            if (
                connectedClients.TryGetValue(id, out WebSocket? client)
                && client.State == WebSocketState.Open
            )
            {
                clients.Add(client);
            }
        }

        await SendMessageAsync(clients, message, connectedClients);
    }

    public static async Task BroadcastMessageToAllExceptAsync(
        IEnumerable<string> ids,
        byte[] message,
        ConcurrentDictionary<string, WebSocket> connectedClients
    )
    {
        var clients = new List<WebSocket>();

        foreach (var kvp in connectedClients)
        {
            if (!ids.Contains(kvp.Key) && kvp.Value.State == WebSocketState.Open)
            {
                clients.Add(kvp.Value);
            }
        }

        await SendMessageAsync(clients, message, connectedClients);
    }

    private static async Task SendMessageAsync(
        IEnumerable<WebSocket> clients,
        byte[] message,
        ConcurrentDictionary<string, WebSocket> connectedClients
    )
    {
        foreach (WebSocket client in clients)
        {
            if (client.State == WebSocketState.Open)
            {
                await client.SendAsync(
                    new ArraySegment<byte>(message),
                    WebSocketMessageType.Binary,
                    true, // Set to false if sending in multiple chunks
                    CancellationToken.None
                );
            }
        }
    }
}
