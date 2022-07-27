using System.Text.Json;

if (args.Length == 0)
{
    Console.WriteLine("Usage: localstr locals.bin/locals.json");
    return;
}

var file = args[0];

switch(Path.GetExtension(file))
{
    case ".bin":
        {
            // parse the localization file
            var localsFile = LocalizationFile.FromFile(file);

            // write to json
            var outputFile = Path.GetFileNameWithoutExtension(file) + ".json";

            File.WriteAllText(outputFile, JsonSerializer.Serialize(localsFile, new JsonSerializerOptions { WriteIndented = true }));

            Console.WriteLine("File written to " + Path.GetFileName(outputFile));
        }
        break;
    case ".json":
        {
            // open our json definition of locals.bin
            var content = File.ReadAllBytes(file);
            var localsFile = JsonSerializer.Deserialize<LocalizationFile>(content);

            if (localsFile.Strings[0].Length > 0)
            {
                Console.WriteLine("First string should be empty");
                return;
            }

            // open output file
            var outputFile = Path.GetFileNameWithoutExtension(file) + ".bin";
            var stream = File.Open(outputFile, FileMode.Truncate);

            // write back to locals.bin in game format
            localsFile.ToFile(stream);
            stream.Close();

            Console.WriteLine("File written to " + Path.GetFileName(outputFile));
        }
        break;
    default:
        Console.WriteLine("Cannot open this file, must be a .bin or .json");
        break;
}