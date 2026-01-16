using System.Collections.Concurrent;
using System.Net.WebSockets;
using Application;
using Domain;

namespace API;

public static class ClientMessageHandler
{
    /// <summary>
    /// Processes a new message received from a client.
    /// Validates the message data and executes the corresponding use case.
    /// </summary>
    public static async Task HandleNewMessage(
        string clientId,
        WebSocketClientMessageDto dto,
        ConcurrentDictionary<string, WebSocket> connectedClients,
        GameStateManager gameStateManager
    )
    {
        Console.WriteLine(
            "[DEBUG] ClientMessageHandler received message: "
                + dto.ToString()
                + " from client "
                + clientId
        );

        string? dtoData = dto.Data;
        WebSocketClientMessageDto.ActionType? dtoAction = dto.Action;

        // Use simple if-else statements for validation
        if (dtoAction == WebSocketClientMessageDto.ActionType.AddNewPlayerToLobby)
        {
            if (
                ClientMessageDataValidator.IsValidJson<AddNewPlayerToLobbyUseCase.Params>(
                    dtoData,
                    out var typedData
                )
            )
            {
                bool success = new AddNewPlayerToLobbyUseCase(gameStateManager).Execute(typedData);
                if (success)
                {
                    Console.WriteLine(
                        "[INFO] Player " + typedData.PlayerId + " added to lobby successfully."
                    );
                }
                else
                {
                    Console.WriteLine(
                        "[WARNING] Player " + typedData.PlayerId + " already exists in lobby."
                    );
                }
            }
            else
            {
                Console.WriteLine(
                    "[WARNING] Invalid data for action AddNewPlayerToLobby from client " + clientId
                );
                return;
            }
        }
        else if (dtoAction == WebSocketClientMessageDto.ActionType.RemovePlayerFromLobby)
        {
            if (
                ClientMessageDataValidator.IsValidJson<RemovePlayerFromLobbyUseCase.Params>(
                    dtoData,
                    out var typedData
                )
            )
            {
                bool success = new RemovePlayerFromLobbyUseCase(gameStateManager).Execute(
                    typedData
                );
                if (success)
                {
                    Console.WriteLine(
                        "[INFO] Player " + typedData.PlayerId + " removed from lobby successfully."
                    );
                }
                else
                {
                    Console.WriteLine(
                        "[WARNING] Player " + typedData.PlayerId + " not found in lobby."
                    );
                }
            }
            else
            {
                Console.WriteLine(
                    "[WARNING] Invalid data for action RemovePlayerFromLobby from client "
                        + clientId
                );
                return;
            }
        }
        else if (dtoAction == WebSocketClientMessageDto.ActionType.MovePlayer)
        {
            if (
                ClientMessageDataValidator.IsValidJson<MovePlayerUseCase.Params>(
                    dtoData,
                    out var typedData
                )
            )
            {
                bool success = new MovePlayerUseCase(gameStateManager).Execute(typedData);
                if (success)
                {
                    Console.WriteLine(
                        "[INFO] Player " + typedData.PlayerId + " moved successfully."
                    );
                }
                else
                {
                    Console.WriteLine(
                        "[WARNING] Failed to move player " + typedData.PlayerId + "."
                    );
                }
            }
            else
            {
                Console.WriteLine(
                    "[WARNING] Invalid data for action MovePlayer from client " + clientId
                );
                return;
            }
        }
        else if (dtoAction == WebSocketClientMessageDto.ActionType.TeleportPlayer)
        {
            if (
                ClientMessageDataValidator.IsValidJson<TeleportPlayerUseCase.Params>(
                    dtoData,
                    out var typedData
                )
            )
            {
                bool success = new TeleportPlayerUseCase(gameStateManager).Execute(typedData);
                if (success)
                {
                    Console.WriteLine(
                        "[INFO] Player " + typedData.PlayerId + " teleported successfully."
                    );
                }
                else
                {
                    Console.WriteLine(
                        "[WARNING] Failed to teleport player " + typedData.PlayerId + "."
                    );
                }
            }
            else
            {
                Console.WriteLine(
                    "[WARNING] Invalid data for action TeleportPlayer from client " + clientId
                );
                return;
            }
        }
        else
        {
            return;
            ;
        }
    }
}
