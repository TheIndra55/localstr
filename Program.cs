using System.Text.Json;
using System.Text.Encodings.Web;

if (args.Length < 2)
{
    Console.WriteLine("Usage: localstr [game] [input file]");
    return 1;
}

var game = args[0];
var file = args[1];

var games = new Dictionary<string, Type>
{
    { "legend", typeof(LegendLocalizationFile) },
    { "rise", typeof(RiseLocalizationFile) }
};

if (!games.ContainsKey(game))
{
    Console.WriteLine("Invalid game, must be: " + string.Join(", ", games.Keys));
    return 1;
}

var type = games[game];

switch(Path.GetExtension(file))
{
    case ".bin":
        {
            // parse the localization file
            var localsFile = (ILocalizationFile)Activator.CreateInstance(type);

            var stream = File.OpenRead(file);
            localsFile.FromFile(stream);

            // write to json
            var outputFile = Path.GetFileNameWithoutExtension(file) + ".json";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            File.WriteAllText(outputFile, JsonSerializer.Serialize(localsFile, type, options));

            Console.WriteLine("File written to " + Path.GetFileName(outputFile));
        }
        break;
    case ".json":
        {
            // open our json definition of locals.bin
            var content = File.ReadAllBytes(file);

            var localsFile = (ILocalizationFile)JsonSerializer.Deserialize(content, type);

            if (localsFile.Strings[0].Length > 0)
            {
                Console.WriteLine("First string should be empty");
                return 1;
            }

            // open output file
            var outputFile = Path.GetFileNameWithoutExtension(file) + ".bin";
            var stream = File.Open(outputFile, FileMode.Create);

            // write back to locals.bin in game format
            localsFile.ToFile(stream);
            stream.Close();

            Console.WriteLine("File written to " + Path.GetFileName(outputFile));
        }
        break;
    default:
        Console.WriteLine("Cannot open this file, must be a .bin or .json");

        return 1;
}

return 0;