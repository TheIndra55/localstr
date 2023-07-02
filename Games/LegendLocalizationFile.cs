using System.Text.Json.Serialization;

class LegendLocalizationFile : ILocalizationFile
{
    [JsonPropertyName("language")]
    public int Language { get; set; }

    [JsonPropertyName("strings")]
    public List<string> Strings { get; set; } = new List<string>();

    public void ToFile(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        writer.Write(Language);
        writer.Write(Strings.Count);

        // calculate header length
        var headerLength = 8 + (Strings.Count * 4);

        // skip past header and start writing
        writer.BaseStream.Position = headerLength;

        int i = 0;
        foreach (var str in Strings)
        {
            if (i == 0)
            {
                i++;
                continue;
            }

            // keep the position of this string
            var offset = writer.BaseStream.Position;

            writer.WriteString(str);

            // keep the current position so we can continue here
            var cursor = writer.BaseStream.Position;

            // seek back to header 
            writer.BaseStream.Position = 8 + (i++ * 4);
            writer.Write((int)offset);

            writer.BaseStream.Position = cursor;
        }
    }

    public void FromFile(Stream stream)
    {
        var reader = new BinaryReader(stream);

        // read the header
        var language = reader.ReadInt32();
        var numStrings = reader.ReadInt32();

        for (var i = 0; i < numStrings; i++)
        {
            var offset = reader.ReadUInt32();

            if (offset == 0)
            {
                Strings.Add(string.Empty);
                continue;
            }

            var cursor = reader.BaseStream.Position;

            // seek to string and read it
            reader.BaseStream.Position = offset;
            Strings.Add(reader.ReadString(true));

            // seek back to header
            reader.BaseStream.Position = cursor;
        }

        Language = language;
    }
}