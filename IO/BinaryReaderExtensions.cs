using System.Text;

static class BinaryReaderExtensions
{
    public static string ReadString(this BinaryReader reader, bool nullTerminated)
    {
        var data = new List<byte>();

        while (reader.ReadByte() != 0)
        {
            reader.BaseStream.Position--;

            data.Add(reader.ReadByte());
        }

        return Encoding.UTF8.GetString(data.ToArray());
    }

    public static void WriteString(this BinaryWriter writer, string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str + "\0");

        writer.Write(bytes);
    }
}