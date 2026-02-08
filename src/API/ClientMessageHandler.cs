using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
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
            "[DEBUG] ClientMessageHandler received binary message with action code: "
                + action
                + " from client "
                + clientId
        );

        if (action == (int)UseCaseActionCodes.AddNewPlayerToLobby)
        {
            string playerName = reader.ReadString();
            var addParams = new AddNewPlayerToLobbyUseCase.Params
            {
                PlayerId = clientId,
                Name = playerName,
            };

            bool success = new AddNewPlayerToLobbyUseCase(gameStateManager).Execute(addParams);

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
            var removeParams = new RemovePlayerFromLobbyUseCase.Params { PlayerId = clientId };
            bool success = new RemovePlayerFromLobbyUseCase(gameStateManager).Execute(removeParams);
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

            var teleportParams = new TeleportPlayerUseCase.Params
            {
                PlayerId = clientId,
                NewPosition = new Vector2(x, y),
                ToScene = (Domain.Scene)sceneId,
            };

            bool success = new TeleportPlayerUseCase(gameStateManager).Execute(teleportParams);

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

            var moveParams = new MovePlayerUseCase.Params
            {
                PlayerId = clientId,
                NewPosition = new Vector2(x, y),
            };

            bool success = new MovePlayerUseCase(gameStateManager).Execute(moveParams);
        }
    }
}
