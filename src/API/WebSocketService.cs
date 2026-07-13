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

            // Force fixed-size GUID string (36 ASCII chars).
            string clientId = Guid.NewGuid().ToString("D");

            _connectedClients[clientId] = webSocket;

            Console.WriteLine(
                $"[INFO] Client connected: {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} with assigned ID {clientId}"
            );

            try
            {
                using (var memoryStream = new MemoryStream())
                using (var writer = new BinaryWriter(memoryStream))
                {
                    writer.Write((short)6); // Write the code "6" as Int16
                    writer.Write(Encoding.ASCII.GetBytes(clientId)); // Write the clientId as raw ASCII bytes

                    await BinaryMessageBroadcaster.BroadcastMessageToSpecificAsync(
                        [clientId],
                        memoryStream.ToArray(),
                        _connectedClients
                    );
                }

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

            Console.WriteLine(
                $"[DEBUG] Received WebSocket message | clientId={clientId} | messageType={result.MessageType} | messageSize={result.Count} bytes"
            );

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                Console.WriteLine($"[DEBUG] Processing binary message | clientId={clientId}");
                await ClientMessageHandler.HandleNewBinaryMessage(
                    clientId: clientId,
                    buffer: buffer,
                    connectedClients: _connectedClients,
                    gameStateManager: _gameStateManager
                );
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                Console.WriteLine($"[INFO] WebSocket close message received | clientId={clientId}");
            }
            else
            {
                Console.WriteLine(
                    $"[WARN] Unsupported WebSocket message type | clientId={clientId} | messageType={result.MessageType}"
                );
            }
        } while (!result.CloseStatus.HasValue);

        Console.WriteLine(
            $"[INFO] WebSocket connection closing | clientId={clientId} | closeStatus={result.CloseStatus}"
        );

        await webSocket.CloseAsync(
            result.CloseStatus.Value,
            result.CloseStatusDescription,
            CancellationToken.None
        );
    }

    public void DisconnectAllClients()
    {
        Console.WriteLine($"[INFO] Disconnecting all clients...");

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
