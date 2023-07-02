using System.Text.Json.Serialization;

class RiseLocalizationFile : ILocalizationFile
{
    [JsonPropertyName("language")]
    public int Language { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }

    [JsonPropertyName("strings")]
    public List<string> Strings { get; set; } = new List<string>();

    public void ToFile(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        var data = Convert.FromBase64String(Data);

        writer.Write(Language);
        writer.Write(data.Length / 4);
        writer.Write(Strings.Count);

        writer.Write(data);

        // calculate header length
        var headerLength = Strings.Count * 8;

        var start = writer.BaseStream.Position;

        // skip past header and start writing
        writer.BaseStream.Position += headerLength;

        int i = 0;
        foreach (var str in Strings)
        {
            if (i == 0)
            {
                i++;
                continue;
            }

            // to match game exporter, might be problematic
            if (str == string.Empty)
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
            writer.BaseStream.Position = start + (i++ * 8);
            writer.Write(offset);

            writer.BaseStream.Position = cursor;
        }
    }

    public void FromFile(Stream stream)
    {
        var reader = new BinaryReader(stream);

        // read the header
        var language = reader.ReadInt32();
        var numFixed = reader.ReadInt32();
        var numStrings = reader.ReadInt32();

        var data = reader.ReadBytes(numFixed * 4);

        for (var i = 0; i < numStrings; i++)
        {
            var offset = reader.ReadInt64();

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

        Data = Convert.ToBase64String(data);
        Language = language;
    }
}