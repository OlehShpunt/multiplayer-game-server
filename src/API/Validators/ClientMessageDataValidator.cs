using System.Text.Json;
using Application;

namespace API;

public static class ClientMessageDataValidator
{
    public static bool IsValidJson<T>(string? data, out T? obj)
    {
        obj = default;

        if (data == null)
        {
            return false;
        }

        try
        {
            obj = JsonSerializer.Deserialize<T>(data);
            return obj != null;
        }
        catch
        {
            return false;
        }
    }
}
