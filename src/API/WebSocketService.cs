using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Application;

namespace API;

public class WebSocketService
{
    private static readonly ConcurrentDictionary<string, WebSocket> _connectedClients =
        new ConcurrentDictionary<string, WebSocket>();
    private static readonly GameStateManager _gameStateManager = new GameStateManager();

    public WebSocketService() { }

    /// <summary>
    /// Connects clients and manages client message listening loop.
    /// </summary>
    public async Task HandleAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            string clientId = Guid.NewGuid().ToString();
            _connectedClients[clientId] = webSocket;

            Console.WriteLine(
                $"[SERVER] Client connected: {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort}"
            );

            // Ensures resources are cleaned up in case an error occurs in the listening loop
            try
            {
                await ListenForClientMessagesAsync(clientId: clientId, webSocket: webSocket);
            }
            catch (Exception error)
            {
                Console.WriteLine($"[WARNING] Error with client {clientId}: {error.Message}");
            }
            finally
            {
                _connectedClients.TryRemove(clientId, out _);
                Console.WriteLine(
                    $"[SERVER] Client disconnected: {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort}"
                );
            }
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }

    private async Task ListenForClientMessagesAsync(string clientId, WebSocket webSocket)
    {
        byte[] buffer = new byte[1024 * 4];
        WebSocketReceiveResult result;
        do
        {
            result = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None
            );

            if (result.MessageType == WebSocketMessageType.Text)
            {
                string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("[DEBUG] New message: " + json);

                WebSocketClientMessageDto receivedDto;
                try
                {
                    receivedDto = JsonSerializer.Deserialize<WebSocketClientMessageDto>(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine(
                        $"[WARNING] Received invalid JSON format from client {clientId}: "
                            + e.Message
                    );
                    continue;
                }

                if (!receivedDto.IsValid())
                {
                    Console.WriteLine(
                        $"[WARNING] Received invalid WebSocketRequestDto from client {clientId}"
                    );
                    continue;
                }
                Console.WriteLine("[DEBUG] WebSocketRequestDto = " + receivedDto.ToString());

                await ClientMessageHandler.HandleNewMessage(
                    clientId: clientId,
                    dto: receivedDto,
                    connectedClients: _connectedClients,
                    gameStateManager: _gameStateManager
                );
            }
        } while (!result.CloseStatus.HasValue);

        await webSocket.CloseAsync(
            result.CloseStatus.Value,
            result.CloseStatusDescription,
            CancellationToken.None
        );
    }

    public void DisconnectAllClients()
    {
        foreach (var client in _connectedClients.Values)
        {
            if (client.State == WebSocketState.Open)
            {
                client.Abort();
            }
        }
        _connectedClients.Clear();
    }

    internal class Broadcaster
    {
        public static async Task BroadcastMessageToAllAsync(WebSocketServerMessageDto message)
        {
            await SendMessageAsync(_connectedClients.Values, message);
        }

        public static async Task BroadcastMessageToSpecificAsync(
            IEnumerable<string> ids,
            WebSocketServerMessageDto message
        )
        {
            IEnumerable<WebSocket> clients = ids.Where(id =>
                    _connectedClients.TryGetValue(id, out WebSocket? client)
                    && client.State == WebSocketState.Open
                )
                .Select(id => _connectedClients[id]);

            await SendMessageAsync(clients, message);
        }

        public static async Task BroadcastMessageExceptAsync(
            string id,
            WebSocketServerMessageDto message
        )
        {
            IEnumerable<WebSocket> clients = _connectedClients
                .Where(client => client.Key != id && client.Value.State == WebSocketState.Open)
                .Select(client => client.Value);

            await SendMessageAsync(clients, message);
        }

        private static async Task SendMessageAsync(
            IEnumerable<WebSocket> clients,
            WebSocketServerMessageDto message
        )
        {
            string serializedMessage = JsonSerializer.Serialize(message);
            byte[] messageBytes = Encoding.UTF8.GetBytes(serializedMessage);

            foreach (WebSocket client in clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(
                        new ArraySegment<byte>(messageBytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                }
            }
        }
    }
}
