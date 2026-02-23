using System.Text;

namespace API;

/// <summary>
/// Utility for reading/writing fixed-length 36-byte ASCII strings (Guid "D" format),
/// without BinaryWriter/BinaryReader's 7-bit length prefix.
/// </summary>
public static class BinaryString36
{
    public const int ByteLength = 36;

    public static void Write(BinaryWriter writer, string value)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(value);

        if (bytes.Length != ByteLength)
        {
            throw new InvalidOperationException(
                $"Expected fixed {ByteLength}-byte ASCII string, got {bytes.Length} bytes. Value=\"{value}\""
            );
        }

        writer.Write(bytes);
    }

    public static string Read(BinaryReader reader)
    {
        byte[] bytes = reader.ReadBytes(ByteLength);

        if (bytes.Length != ByteLength)
        {
            throw new EndOfStreamException(
                $"Expected {ByteLength} bytes for fixed string, got {bytes.Length}."
            );
        }

        return Encoding.ASCII.GetString(bytes);
    }
}
