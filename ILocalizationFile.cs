interface ILocalizationFile
{
    public List<string> Strings { get; }

    public void ToFile(Stream stream);
    public void FromFile(Stream stream);
}