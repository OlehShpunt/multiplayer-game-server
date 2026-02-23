using Domain;

namespace API;

public static class BinaryMessageBuilder
{
    public static byte[] CreatePlayerJoinedMessage(string playerId, string playerName)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((short)BroadcastActionCodes.PlayerJoinedLobby);
        writer.Write(playerId);
        writer.Write(playerName);

        return memoryStream.ToArray();
    }

    public static byte[] CreatePlayerLeftMessage(string playerId)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((short)BroadcastActionCodes.PlayerLeftLobby);
        writer.Write(playerId);

        return memoryStream.ToArray();
    }

    public static byte[] CreatePlayerMovedMessage(string playerId, float newX, float newY)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((short)BroadcastActionCodes.PlayerMoved);
        writer.Write(playerId);
        writer.Write(newX);
        writer.Write(newY);

        return memoryStream.ToArray();
    }

    public static byte[] CreatePlayerTeleportedMessage(
        string playerId,
        float newX,
        float newY,
        Scene newScene
    )
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((short)BroadcastActionCodes.PlayerTeleported);
        writer.Write(playerId);
        writer.Write(newX);
        writer.Write(newY);
        writer.Write((short)newScene);

        return memoryStream.ToArray();
    }

    public static byte[] CreateErrorMessage(string errorMessage)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((short)BroadcastActionCodes.Error);
        writer.Write(errorMessage);

        return memoryStream.ToArray();
    }

    public static byte[] CreateSuccessMessage(string successMessage)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((short)BroadcastActionCodes.Success);
        writer.Write(successMessage);

        return memoryStream.ToArray();
    }
}
