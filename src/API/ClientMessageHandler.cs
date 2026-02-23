using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using API;
using Application;

namespace API;

public static class ClientMessageHandler
{
    /// <summary>
    /// Calls use cases depending on the action code that is specified in the binary message.
    /// </summary>
    public static async Task HandleNewBinaryMessage(
        string clientId,
        byte[] buffer,
        ConcurrentDictionary<string, WebSocket> connectedClients,
        GameStateManager gameStateManager
    )
    {
        using var ms = new MemoryStream(buffer);
        using var reader = new BinaryReader(ms);

        int action = reader.ReadInt16();

        Console.WriteLine(
            $"[DEBUG] Binary message received | clientId={clientId} | actionCode={action} | bufferLength={buffer?.Length ?? 0}"
        );

        if (action == (int)UseCaseActionCodes.AddNewPlayerToLobby)
        {
            string playerName = reader.ReadString();

            Console.WriteLine(
                $"[DEBUG] Action=AddNewPlayerToLobby | playerId={clientId} | playerName=\"{playerName}\""
            );

            var addParams = new AddNewPlayerToLobbyUseCase.Params
            {
                PlayerId = clientId,
                Name = playerName,
            };

            bool success = new AddNewPlayerToLobbyUseCase(gameStateManager).Execute(addParams);

            Console.WriteLine(
                $"[DEBUG] Action=AddNewPlayerToLobby | success={success} | playerId={clientId} | playerName=\"{playerName}\""
            );

            if (success)
            {
                await BinaryMessageBroadcaster.BroadcastMessageToAllExceptAsync(
                    [clientId],
                    BinaryMessageBuilder.CreatePlayerJoinedMessage(clientId, playerName),
                    connectedClients
                );
                await BinaryMessageBroadcaster.BroadcastMessageToSpecificAsync(
                    [clientId],
                    BinaryMessageBuilder.CreateSuccessMessage("Successfully added to lobby"),
                    connectedClients
                );
            }
            else
            {
                await BinaryMessageBroadcaster.BroadcastMessageToSpecificAsync(
                    [clientId],
                    BinaryMessageBuilder.CreateErrorMessage("Failed to add to lobby"),
                    connectedClients
                );
            }
        }
        else if (action == (int)UseCaseActionCodes.RemovePlayerFromLobby)
        {
            Console.WriteLine($"[DEBUG] Action=RemovePlayerFromLobby | playerId={clientId}");

            var removeParams = new RemovePlayerFromLobbyUseCase.Params { PlayerId = clientId };
            bool success = new RemovePlayerFromLobbyUseCase(gameStateManager).Execute(removeParams);

            Console.WriteLine(
                $"[DEBUG] Action=RemovePlayerFromLobby | success={success} | playerId={clientId}"
            );

            if (success)
            {
                await BinaryMessageBroadcaster.BroadcastMessageToAllExceptAsync(
                    [clientId],
                    BinaryMessageBuilder.CreatePlayerLeftMessage(clientId),
                    connectedClients
                );
                await BinaryMessageBroadcaster.BroadcastMessageToSpecificAsync(
                    [clientId],
                    BinaryMessageBuilder.CreateSuccessMessage("Successfully removed from lobby"),
                    connectedClients
                );
            }
            else
            {
                await BinaryMessageBroadcaster.BroadcastMessageToSpecificAsync(
                    [clientId],
                    BinaryMessageBuilder.CreateErrorMessage("Failed to remove from lobby"),
                    connectedClients
                );
            }
        }
        else if (action == (int)UseCaseActionCodes.TeleportPlayer)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            int sceneId = reader.ReadInt16();

            Console.WriteLine(
                $"[DEBUG] Action=TeleportPlayer | playerId={clientId} | x={x} | y={y} | sceneId={sceneId} | toScene={(Domain.Scene)sceneId}"
            );

            var teleportParams = new TeleportPlayerUseCase.Params
            {
                PlayerId = clientId,
                NewPosition = new Vector2(x, y),
                ToScene = (Domain.Scene)sceneId,
            };

            bool success = new TeleportPlayerUseCase(gameStateManager).Execute(teleportParams);

            Console.WriteLine(
                $"[DEBUG] Action=TeleportPlayer | success={success} | playerId={clientId} | x={x} | y={y} | sceneId={sceneId}"
            );

            if (success)
            {
                await BinaryMessageBroadcaster.BroadcastMessageToAllAsync(
                    BinaryMessageBuilder.CreatePlayerTeleportedMessage(
                        clientId,
                        x,
                        y,
                        (Domain.Scene)sceneId
                    ),
                    connectedClients
                );
            }
            else
            {
                await BinaryMessageBroadcaster.BroadcastMessageToSpecificAsync(
                    [clientId],
                    BinaryMessageBuilder.CreateErrorMessage("Failed to teleport"),
                    connectedClients
                );
            }
        }
        else if (action == (int)UseCaseActionCodes.MovePlayer)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();

            Console.WriteLine($"[DEBUG] Action=MovePlayer | playerId={clientId} | x={x} | y={y}");

            var moveParams = new MovePlayerUseCase.Params
            {
                PlayerId = clientId,
                NewPosition = new Vector2(x, y),
            };

            bool success = new MovePlayerUseCase(gameStateManager).Execute(moveParams);

            Console.WriteLine(
                $"[DEBUG] Action=MovePlayer | success={success} | playerId={clientId}"
            );

            if (success)
            {
                await BinaryMessageBroadcaster.BroadcastMessageToAllExceptAsync(
                    [clientId],
                    BinaryMessageBuilder.CreatePlayerMovedMessage(clientId, x, y),
                    connectedClients
                );
            }
            else
            {
                await BinaryMessageBroadcaster.BroadcastMessageToSpecificAsync(
                    [clientId],
                    BinaryMessageBuilder.CreateErrorMessage("Failed to move player"),
                    connectedClients
                );
            }
        }
        else
        {
            Console.WriteLine(
                $"[WARN] Unknown action code | clientId={clientId} | actionCode={action}"
            );
        }
    }
}
