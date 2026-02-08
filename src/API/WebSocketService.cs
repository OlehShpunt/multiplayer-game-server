using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
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
    public async Task HandleNewConnectionAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            string clientId = Guid.NewGuid().ToString();
            _connectedClients[clientId] = webSocket;

            Console.WriteLine(
                $"[INFO] Client connected: {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} with assigned ID {clientId}"
            );

            try
            {
                await BinaryMessageBroadcaster.BroadcastMessageToSpecificAsync(
                    [clientId],
                    Encoding.UTF8.GetBytes(clientId),
                    _connectedClients
                );
                await ListenForClientMessagesAsync(clientId: clientId, webSocket: webSocket);
            }
            catch (Exception error)
            {
                Console.WriteLine(
                    $"[WARNING] Error with client {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} (ID:{clientId}): {error.Message}"
                );
            }
            // Clean up resources
            finally
            {
                _connectedClients.TryRemove(clientId, out _);
                Console.WriteLine(
                    $"[INFO] Client disconnected: {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} (ID:{clientId})"
                );
            }
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }

    private static async Task ListenForClientMessagesAsync(string clientId, WebSocket webSocket)
    {
        byte[] buffer = new byte[1024 * 4]; // TODO: based on message sizes, calculate how many bytes are actually needed for the buffer
        WebSocketReceiveResult result;
        do
        {
            result = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None
            );

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                await ClientMessageHandler.HandleNewBinaryMessage(
                    clientId: clientId,
                    buffer: buffer,
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
}
